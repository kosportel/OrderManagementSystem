# ORDER MANAGEMENT SYSTEM

## The Concept

Having the given requirements as a baseline, I designed a system inspired by the **Wolt app**, but tailored for a **single restaurant**.

The core workflow includes the following key areas:

- A **customer** registers in the system, creating a user profile and adding extra details like address and phone number.
- The customer places an **order** by selecting items from the predefined menu.
- The system **tracks** the progress of the order. If the order is for delivery, it is assigned to a **delivery employee**, as soon as it is prepared.
- The **delivery employee** updates the status of the order as it moves through the delivery process until it is completed.

Additionally, there is an **administration panel** for managing users, the menu, and the orders.

## The Architecture

I applied a **Clean Architecture** pattern, leveraging its benefits of modularity and maintainability. The project is structured into the following layers:

- **Presentation (API)**
- **Application Layer**
- **Domain Layer**
- **Infrastructure Layer**

To ensure loose coupling, data is shared across these layers using **DTOs, Domain Models, and Entities**.

### **Key Technical Decisions**

- Implemented **Repository Pattern** to abstract database interactions and promote maintainability.
- Used **SQLite** for portability, following a **Code-First** approach with migrations.
- On application startup, **database migrations** are automatically applied since no CI/CD pipeline is currently in place.
- **FluentValidation** is used for validating DTOs.
- **AutoMapper** facilitates object transformation between layers.
- **JWT authentication** is implemented for securing API endpoints.
- **Serilog** is integrated for structured logging.

## Setup & Running Instructions

### **Steps to Run the Application**

1. Clone the repository:
   ```sh
   git clone https://github.com/kosportel/OrderManagementSystem.git
   ```
2. Navigate to the API project:
   ```sh
   cd OrderManagementSystem/src/OrderManagement.Api
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```
4. Run the application:
   ```sh
   dotnet run
   ```
5. Open **Swagger UI** to explore the API:
   ```
   https://localhost:7125/swagger
   ```

### **Authentication**

All API endpoints require authentication. Use the following credentials to log in:

- **Username:** agile_admin@actors.com
- **Password:** 1234567890

Use the **JWT token** from login to authenticate subsequent requests in Swagger or Postman.

### **Sample Flow**
- Login using the credentials above
```
{
  "email": "agile_admin@actors.com",
  "password": "1234567890"
}
```

### **Pickup Order**
- Set a new order for Pickup [Post]https://localhost:7125/api/orders
```
{
    "customerId": 1,
    "orderTypeId": "Pickup",
    "addressId": 2,
    "notes": "Please deliver ASAP",
    "orderItems": [
        { "menuItemId": 1, "quantity": 2, "price": 8.99 },
        { "menuItemId": 3, "quantity": 1, "price": 7.99 }
    ]
}
```
- Check that the order is created [Get]https://localhost:7125/api/orders/{id}
- Check the status [Get]https://localhost:7125/api/orders/{id}/status
- Move the status to the next step [Post]https://localhost:7125/api/orders/id/status
- Check the status [Get]https://localhost:7125/api/orders/id/status
- Move the status to the next step until the order is ready [Post]https://localhost:7125/api/orders/id/status

### **Delivery Order**
- Set a new order for Pickup [Post]https://localhost:7125/api/orders
```
{
    "customerId": 1,
    "orderTypeId": "Delivery",
    "addressId": 2,
    "notes": "",
    "orderItems": [
        { "menuItemId": 1, "quantity": 2, "price": 8.99 },
        { "menuItemId": 3, "quantity": 1, "price": 7.99 }
    ]
}
```
- Check that the order is created [Get]https://localhost:7125/api/orders/{id}
- Check the status [Get]https://localhost:7125/api/orders/{id}/status
- Move the status to the next step [Post]https://localhost:7125/api/orders/id/status
- Check the status [Get]https://localhost:7125/api/orders/id/status
- Move the status to the next step until the order is ready [Post]https://localhost:7125/api/orders/id/status
- Prepare the delivery - Check for idle people [Get]https://localhost:7125/api/delivery/idle
- Assign the order to a Delivery person [Put]https://localhost:7125/api/orders/12/assign/3
- Check that the order is assigned to the Delivery person [Get]https://localhost:7125/api/delivery/assignments/user/3
- The person starts the Delivery [Put] https://localhost:7125/api/delivery/orders/12/startDelivery?userId=3
- The person ends the Delivery [Put]https://localhost:7125/api/delivery/orders/12/close
```
{
  "userId": 3,
  "isDelivered": true,
  "notes": "string"
}
```

## Future Enhancements

As an initial version, the application is designed to be **scalable** and **extensible**, while remaining simple to align with current requirements. Several improvements can be made in future versions.

### **Business Expansions**

- **Loyalty Rewards & Discounts** for returning customers.
- **Promotions & Campaigns** to boost sales.
- **Online Payments** integration.
- **Delivery Tracking & Maps** for real-time order tracking.
- **User Notifications** (SMS, Email, Push notifications).
- **Estimated Delivery Time Predictions** based on real-time data.
- **ML** for forcasting user choises.

### **Technical Improvements**

- **Health Checks & Monitoring** for better observability.
- **OpenTelemetry Integration** for tracing API requests.
- **Application Metrics & Insights** for performance analysis.
- **Event-Driven Architecture** using a **Message Queue** (RabbitMQ, Kafka) to track order progress & delivery updates.
- **Optimized Query Performance** by analyzing query execution and caching frequently accessed data.
- **Automated Testing**: Expand unit, integration, and load testing coverage.

## API Documentation

Swagger provides interactive API documentation at:

```
https://localhost:7125/swagger
```

---

This documentation provides a high-level overview of the system. Refer to **code comments and Swagger documentation** for further API usage details.
