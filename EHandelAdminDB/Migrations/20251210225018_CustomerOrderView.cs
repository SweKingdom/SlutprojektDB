using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHandelAdminDB.Migrations
{
    /// <inheritdoc />
    public partial class CustomerOrderView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove old version of the view if it exists
            migrationBuilder.Sql("DROP VIEW IF EXISTS CustomerOrderCountView;");
            
            migrationBuilder.Sql(@"
                CREATE VIEW IF NOT EXISTS CustomerOrderCountView AS
                SELECT
                    c.CustomerId,
                    c.Name AS CustomerName,
                    c.Email AS CustomerEmail,
                    IFNULL(COUNT(o.OrderId), 0) AS NmbrOfOrders
                FROM Customers c
                LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
                GROUP BY c.CustomerId, c.Name, c.Email;
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS CustomerOrderCountView;");
        }
    }
}