# Patient Scorecard API

## Overview
The Patient Scorecard API is designed to manage user data, dashboard data, and various operations related to patient scorecards. It includes functionalities for user authentication, user management, and dashboard data retrieval.

## Project Structure

### DataAccess
- **UserDataAccess.cs**: Handles database operations related to user data, such as login, user listing, adding/updating users, and more.
- **DashboardDataAccess.cs**: Manages database operations for dashboard data, including fetching dashboard details, adherence breakdown, sales order details, and more.
- **Interface/IUserDataAccess.cs**: Interface for user data access operations.
- **Interface/IDashboardDataAccess.cs**: Interface for dashboard data access operations.

### BusinessLayer
- **User.cs**: Contains business logic for user-related operations, such as login, user listing, adding/updating users, and more.
- **Dashboard.cs**: Implements business logic for dashboard-related operations, including fetching dashboard details, adherence breakdown, sales order details, and more.
- **Interface/IUser.cs**: Interface for user business logic operations.
- **Interface/IDashboard.cs**: Interface for dashboard business logic operations.

### Controllers
- **UserController.cs**: API controller for handling user-related requests, such as login, user listing, adding/updating users, and more.
- **DashboardController.cs**: API controller for managing dashboard-related requests, including fetching dashboard details, adherence breakdown, sales order details, and more.
