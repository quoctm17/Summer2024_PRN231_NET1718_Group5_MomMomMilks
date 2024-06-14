
# MomMomMilks

Welcome to **MomMomMilks** - a web application platform dedicated to selling milk for pregnant mothers and babies. This project aims to provide a seamless and efficient shopping experience for our users.

## Table of Contents

- [Introduction](#introduction)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
  - [Backend](#backend)
  - [Frontend](#frontend)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgements](#acknowledgements)

## Introduction

**MomMomMilks** is designed to provide a robust platform for selling milk products to pregnant mothers and babies. Our application leverages modern web technologies to ensure high performance, security, and scalability.

## Technologies Used

- **Backend:**
  - ASP.NET Core Web API
  - OData
  - JWT (JSON Web Tokens)
  - PayOS
  - gRPC
  - 3-Layer Architecture (DAO -> Repository -> Service -> API)

- **Frontend:**
  - Razor Pages
  - JavaScript
  - Ajax

## Project Structure

```
MomMomMilks
├── BE (Backend)
│   ├── Controllers
│   ├── DAO
│   ├── Repository
│   ├── Service
│   ├── Models
│   └── Interfaces
└── FE (Frontend)
    ├── Pages
    ├── wwwroot
    ├── Scripts
    ├── Styles
    └── Models
```

## Getting Started

### Prerequisites

Before you begin, ensure you have met the following requirements:
- .NET Core SDK
- Node.js (for frontend dependencies)
- A code editor like Visual Studio or VS Code
- Git

### Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/yourusername/MomMomMilks.git
    ```

2. **Navigate to the backend project directory and restore dependencies:**

    ```bash
    cd MomMomMilks/BE
    dotnet restore
    ```

3. **Navigate to the frontend project directory and install dependencies:**

    ```bash
    cd ../FE
    npm install
    ```

## Usage

### Backend

1. **Run the backend:**

    ```bash
    cd MomMomMilks/BE
    dotnet run
    ```

2. **API Documentation:**
   - Navigate to `https://localhost:5001/swagger` to view the API documentation.

### Frontend

1. **Run the frontend:**

    ```bash
    cd MomMomMilks/FE
    dotnet run
    ```

2. **Access the application:**
   - Navigate to `https://localhost:5000` to access the frontend application.

## Contributing

Contributions are always welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature/YourFeature`).
6. Open a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements

- Special thanks to all the contributors who have helped in the development of this project.
- Thanks to the open-source community for providing the tools and frameworks used in this project.

---

*This README file was generated with ❤️ by the MomMomMilks team.*
```
