using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Linq;

namespace Scorecard.API.DataAccess
{
	public class Utilities
	{
	}

	public class DataClasses
	{
		SqlConnection Connection;
		public IConfiguration _configuration { get; }

		public DataClasses(IConfiguration configuration) => _configuration = configuration;

		public DataTable GetDataTable(string StoredProcedure, List<SqlParameter> sqlParameterCollection)
		{
			Connection = new SqlConnection(_configuration.GetConnectionString("DbConnection"));

			DataTable Dt = new DataTable();
			try
			{
				SqlCommand Cmd = new SqlCommand();

				Cmd.Connection = Connection;
				Cmd.CommandText = StoredProcedure;

				Cmd.CommandType = CommandType.StoredProcedure;
				Cmd.CommandTimeout = 300;

				if (sqlParameterCollection != null)
				{
					Cmd.Parameters.AddRange(sqlParameterCollection.ToArray());
				}

				Connection.Open();

				Dt.Load(Cmd.ExecuteReader());

				Connection.Close();
				Cmd.Parameters.Clear();
				Cmd.Dispose();
				Cmd = null;
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				if (Connection.State != ConnectionState.Closed)
				{
					Connection.Close();
				}
				Connection.Dispose();
				Connection = null;
			}
			return Dt;
		}

		public DataSet GetDataSet(string SQL, List<SqlParameter> sqlParameterCollection)
		{
			Connection = new SqlConnection(_configuration.GetConnectionString("DbConnection"));

			DataSet ds = new DataSet();
			try
			{
				SqlCommand cmd = new SqlCommand();

				cmd.Connection = Connection;
				cmd.CommandText = SQL;

				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandTimeout = 30000;

				if (sqlParameterCollection != null)
				{
					cmd.Parameters.AddRange(sqlParameterCollection.ToArray());
				}

				Connection.Open();

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
				sqlDataAdapter.SelectCommand = cmd;
				sqlDataAdapter.Fill(ds);

				Connection.Close();
				cmd.Parameters.Clear();
				cmd.Dispose();
				cmd = null;
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				if (Connection.State != ConnectionState.Closed)
				{
					Connection.Close();
				}
				Connection.Dispose();
				Connection = null;
			}
			return ds;
		}

		public int JustExecute(string SQL, List<SqlParameter> sqlParameterCollection)
		{
			Connection = new SqlConnection(_configuration.GetConnectionString("DbConnection"));
			int i = 0;

			try
			{
				SqlCommand cmd = new SqlCommand();

				cmd.Connection = Connection;
				cmd.CommandText = SQL;

				cmd.CommandType = CommandType.StoredProcedure;

				if (sqlParameterCollection != null)
				{
					cmd.Parameters.AddRange(sqlParameterCollection.ToArray());
				}

				Connection.Open();

				i = cmd.ExecuteNonQuery();

				Connection.Close();
				cmd.Parameters.Clear();
				cmd.Dispose();
				cmd = null;
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				if (Connection.State != ConnectionState.Closed)
				{
					Connection.Close();
				}
				Connection.Dispose();
			}
			return i;
		}

		public int JustExecuteScalar(string SQL, List<SqlParameter> sqlParameterCollection)
		{
			Connection = new SqlConnection(_configuration.GetConnectionString("DbConnection"));
			int i = 0;

			try
			{
				SqlCommand cmd = new SqlCommand();

				cmd.Connection = Connection;
				cmd.CommandText = SQL;

				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Clear();

				if (sqlParameterCollection != null)
				{
					cmd.Parameters.AddRange(sqlParameterCollection.ToArray());
				}

				Connection.Open();

				i = Convert.ToInt32(cmd.ExecuteScalar());

				Connection.Close();
				cmd.Dispose();
				cmd = null;
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				if (Connection.State != ConnectionState.Closed)
				{
					Connection.Close();
				}
				Connection.Dispose();
				Connection = null;
			}
			return i;
		}

		public bool IsValidRequest(string UserId, string AccessToken)
		{
			string StoredProcedure = @"dbo.SP_Check_UserRequest";
			List<SqlParameter> ParamCollection = new List<SqlParameter>();
			ParamCollection.Add(new SqlParameter("UserId", UserId.HandleDBNull()));
			ParamCollection.Add(new SqlParameter("AccessToken", AccessToken.HandleDBNull()));
			int Result = JustExecuteScalar(StoredProcedure, ParamCollection);

			if (Result == 1)
				return true;
			else
				return false;
		}

		public bool CheckUserHasAccessPermission(string UserId, int PermissionId)
		{
			string StoredProcedure = @"dbo.SP_Check_UserHasAccessPermission";
			List<SqlParameter> ParamCollection = new List<SqlParameter>();
			ParamCollection.Add(new SqlParameter("UserId", UserId.HandleDBNull()));
			ParamCollection.Add(new SqlParameter("PermissionId", PermissionId.HandleDBNull()));
			int Result = JustExecuteScalar(StoredProcedure, ParamCollection);

			if (Result == 1)
				return true;
			else
				return false;
		}

		public string BuildSqlCommand(List<SqlParameter> ParamCollection, string StoredProcedure)
		{
			int Count = 1;
			string SqlCommand = string.Format(" DECLARE @{0} int", StoredProcedure);
			SqlCommand += string.Format(" EXEC @{0} = [dbo].[{1}] ", StoredProcedure, StoredProcedure);

			foreach (var Param in ParamCollection)
			{
				if (ParamCollection.Count > 1 && Count < ParamCollection.Count)
				{
					SqlCommand += string.Format(" @{0} = '{1}', ", Param.ParameterName, Param.SqlValue);
					Count++;
				}
				else
					SqlCommand += string.Format(" @{0} = '{1}' ", Param.ParameterName, Param.SqlValue);
			}
			SqlCommand += string.Format(" SELECT 'Return Value' = @{0} ", StoredProcedure);
			return SqlCommand;

		}

		public string ReadContentFromTemplate(string Type)
		{
			string FolderPath = "\\Content\\EmailTemplate\\";
			string FileName = string.Empty;
			if (Type == "NewRegistration" || Type == "ResendEmail")
				FileName = "EmailTemplateForLoginDetail.html";
			else if (Type == "ResetPassword")
				FileName = "EmailTemplateForResetPassword.html";
			else if (Type == "Instant")
				FileName = "EmailTemplateForSingleReferralScreener.html";

			StreamReader ObjReader;
			ObjReader = new StreamReader(string.Format("{0}{1}{2}", Directory.GetCurrentDirectory(), FolderPath, FileName));
			string EmailContent = ObjReader.ReadToEnd();
			ObjReader.Close();
			ObjReader.Dispose();
			return EmailContent;
		}
	}

	public static class StaticUtilities
	{
		public static object HandleDBNull(this object Value)
		{
			if (Value == null)
			{
				return DBNull.Value;
			}
			return Value;
		}

		public static JArray DataTableToJson(DataTable source)
		{
			JArray result = new JArray();
			JObject row;
			foreach (System.Data.DataRow dr in source.Rows)
			{
				row = new JObject();
				foreach (System.Data.DataColumn col in source.Columns)
				{
					row.Add(col.ColumnName.Trim(), JToken.FromObject(dr[col]));
				}
				result.Add(row);
			}
			return result;
		}

		public static List<T> DataTableToList<T>(DataTable dt)
		{
			List<T> data = new List<T>();
			foreach (DataRow row in dt.Rows)
			{
				T item = GetItem<T>(row);
				data.Add(item);
			}
			return data;
		}

		private static T GetItem<T>(DataRow dr)
		{
			Type temp = typeof(T);
			T obj = Activator.CreateInstance<T>();

			foreach (DataColumn column in dr.Table.Columns)
			{
				foreach (PropertyInfo pro in temp.GetProperties())
				{
					if (pro.Name == column.ColumnName)
					{
						object value = dr[column.ColumnName];
						if (value == DBNull.Value) value = null;
						pro.SetValue(obj, value, null);
					}
					else
						continue;
				}
			}
			return obj;
		}

	}

	public static class EncryptionHelper
	{
		public static string GenerateToken(this string Value)
		{
			if (Value.Replace(" ", "").Trim() != "")
			{
				int SaltByteSize = 35;
				int Pbkdf2Iterations = 1000;
				int HashByteSize = 30;
				var cryptoProvider = new RNGCryptoServiceProvider();
				byte[] salt = new byte[SaltByteSize];
				cryptoProvider.GetBytes(salt);

				var hash = Pbkdf_GetPbkdf2Bytes(Value, salt, Pbkdf2Iterations, HashByteSize);
				return Pbkdf2Iterations + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
			}
			return Value;
		}
		public static bool ValidateToken(this string password, string correctHash)
		{
			int IterationIndex = 0;
			int SaltIndex = 1;
			int Pbkdf2Index = 2;
			char[] delimiter = { ':' };
			var split = correctHash.Split(delimiter);
			var iterations = Int32.Parse(split[IterationIndex]);
			var salt = Convert.FromBase64String(split[SaltIndex]);
			var hash = Convert.FromBase64String(split[Pbkdf2Index]);

			var testHash = Pbkdf_GetPbkdf2Bytes(password, salt, iterations, hash.Length);
			return Pbkdf_SlowEquals(hash, testHash);
		}
		private static bool Pbkdf_SlowEquals(byte[] a, byte[] b)
		{
			var diff = (uint)a.Length ^ (uint)b.Length;
			for (int i = 0; i < a.Length && i < b.Length; i++)
			{
				diff |= (uint)(a[i] ^ b[i]);
			}
			return diff == 0;
		}
		private static byte[] Pbkdf_GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
		{
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
			pbkdf2.IterationCount = iterations;
			return pbkdf2.GetBytes(outputBytes);
		}

	}

	public class PublicIPAddress
	{
		public string GetPublicIpAddress()
		{
			WebClient client = new WebClient();
			return client.DownloadString("https://api.ipify.org");
		}
	}
	public class SendEmailForCommon
	{
		public IConfiguration _configuration { get; }
		public SendEmailForCommon(IConfiguration configuration) => _configuration = configuration;
		public bool SendEmail(string Subject, string Message, string EmailTo)
		{
			bool IsEmailSent = false;
			string SMTP_Client = _configuration.GetSection("EmailConfiguration:SMTP_Client").Value;
			string SMTP_Email = _configuration.GetSection("EmailConfiguration:SMTP_Email").Value;
			string SMTP_Password = _configuration.GetSection("EmailConfiguration:SMTP_Password").Value;
			string SMTP_FromEmail = _configuration.GetSection("EmailConfiguration:SMTP_FromEmail").Value;

			SmtpClient EmailClient = new SmtpClient(SMTP_Client, 587);
			EmailClient.EnableSsl = true;
			EmailClient.Credentials = new NetworkCredential(SMTP_Email, SMTP_Password);

			MailMessage newMessage = new MailMessage();
			newMessage.From = new MailAddress(SMTP_FromEmail, "Referral Portal", System.Text.Encoding.UTF8);

			newMessage.To.Add(new MailAddress(EmailTo));

			newMessage.Body = Message;
			newMessage.BodyEncoding = System.Text.Encoding.UTF8;

			newMessage.Subject = Subject;
			newMessage.IsBodyHtml = true;
			newMessage.SubjectEncoding = System.Text.Encoding.UTF8;

			try
			{
				EmailClient.Send(newMessage);
				IsEmailSent = true;
			}
			catch
			{
				IsEmailSent = false;
			}

			return IsEmailSent;
		}
	}

	public class GenerateRandomPassword
	{

		public static Random random = new Random();
		public string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!%#";
			return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}

	}
}
