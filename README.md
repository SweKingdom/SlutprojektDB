This is a small console app that I built to practice C#, Entity Framework Core and SQLite. It simulates a basic shop service where you can manage customers, orders, products and categories. Data is built to be stored in SQLite and there are some seeded data for easy start. Socialsecurity number is not saved and is hashed and salted. Email is encrypted.

-- Run the application --

1.	Open Console App and navigate with cd to your map containing your program files
2.	Clone the repository: git clone https://github.com/SweKingdom/SlutprojektDB
3.	Open the program by running the EHandelAdminDB.sln program
4.	Run the application: Start Without Debugging (Ctrl+F5)
5.	The console app opens automaticly and the main Meny is open


The database shop.db will be created automatically when the application is runned.
Seeded Data is added if database is empty.

-- Features --
Customer Management
⦁	Add new customers
⦁	List all customers
⦁	View customers with total number of orders
⦁	SSN hashing + salting
⦁	Email encryption and decryption

Product management
⦁	Add, edit, delete and read products
⦁	Search products
⦁	View total quantity sold by SQL view

Category management
⦁	Add, edit, delete and list categories
⦁	Search categories

Order management
⦁	Create new customer orders
⦁	Add multiple order rows per order
⦁	Change order status (Pending, Paid, Shipped)
⦁	pagination support when listing orders
⦁	List orders per customer
⦁	List orders containing a specific product
