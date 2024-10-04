using System.ComponentModel.DataAnnotations.Schema;

namespace AspnetCore_Redis.Models
{
    [Table("orders")]
    public class Order
    {
        [Column("orderid")]
        public int OrderId { get; set; }

        [Column("customerid")]
        public int CustomerId { get; set; }

        [Column("employeeid")]
        public int EmployeeId { get; set; }

        [Column("orderdate")]
        public DateTime OrderDate { get; set; }

        [Column("shipperid")]
        public int ShipperId { get; set; }
    }
}
