using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
/*
 * En view är en sparad SELECT-fråga i db.
 * - Förenklar komplexa JOINs
 * - Ger oss färdiga summeringar
 * - Slipper skriva samma SQL om och om igen
 * - Säker visning av information och prestandan blir bättre
 *
 */

namespace EHandelAdminDB.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersummeryView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS OrderSummaryView AS
            SELECT
                o.OrderId,
                o.OrderDate,
                c.Name AS CustomerName,
                c.Email AS CustomerEmail,
                IFNULL(SUM(orw.Quantity * orw.UnitPrice), 0) AS TotalPrice
            FROM Orders o
            JOIN Customers c ON c.CustomerId = o.CustomerId
            LEFT JOIN OrderRows orw ON orw.OrderId = o.OrderId
            GROUP BY o.OrderId, o.OrderDate, c.Name, c.Email;
        ");

            
            // AFTER INSERT
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Insert
            AFTER INSERT ON OrderRows
            BEGIN
                UPDATE Orders
                SET TotalAmount = (
                                    Select IFNULL(SUM(Quantity * UnitPrice), 0)
                                    FROM OrderRows
                                    WHERE OrderId = NEW.OrderId
                                )
                                WHERE OrderId = NEW.OrderId;
                END;
            ");
            // AFTER UPDATAE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Update
            After UPDATE ON OrderRows
            BEGIN
            UPDATE Orders
                SET TotalAmount = (
                                    SELECT IFNULL (SUM(Quantity * UnitPrice), 0)
                                    FROM OrderRows
                                    WHERE OrderId = NEW.OrderId
                                    )
                WHERE OrderId = NEW.OrderId;
            END;     
            ");
            
            // AFTER DELETE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Delete
            AFTER DELETE ON OrderRows
            BEGIN
            UPDATE Orders
                SET TotalAmount = (
                                    SELECT IFNULL (SUM(Quantity * UnitPrice), 0)
                                    FROM OrderRows
                                    WHERE OrderId = NEW.OrderId
                                    )
                                WHERE OrderId = NEW.OrderId;
            END;
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS OrderSummaryView
            ");
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS trg_OrderRow_Insert
            ");
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS trg_OrderRow_Update
            ");
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS trg_OrderRow_Delete
            ");
        }
    }
}