using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using BlazorServerApp.Data;
using Mysqlx.Crud;
using System.Security.Cryptography.X509Certificates;


public class Dbmethods
{
    static IDbConnection db = new MySqlConnection("server=127.0.0.1;User ID=root;database=olfdb");

    static public void Addcustomerdb(Customer newcustomer)
    {
        
        string sql2 = "INSERT INTO customers (name, adress, email, phone) VALUES (@name, @adress, @email, @phone)";
        var rowsAffected = db.Execute(sql2, newcustomer);
    }

    static public List<Customer> PrintCust()
    {
        var sql = "SELECT id AS customerid, name, adress, email, phone FROM Customers";
        var customers = db.Query<Customer>(sql).ToList();
        return customers;
    }

    static public List<Customer> FindCust(string mycust)
    {
       var sql = "SELECT id AS customerid, name, adress, email, phone FROM customers " + "WHERE Customers.name=" +"\'" +mycust + "\';";
        var customers = db.Query<Customer>(sql).ToList();
        return customers; 

    }

    static public List<Orders> MyOrders(int custid)
    {
        var sql = "SELECT id, odate, status FROM orders " + "WHERE orders.customerid=" + custid + ";";
        var myorders = db.Query<Orders>(sql).ToList();
        return myorders; 
    }

    static public List<Orderrowstwo> MyOrderrows(int myorder)
    {
        var sql = "SELECT products.id, combo.color, combo.size, products.name, orders.odate, orders.status, orderrows.quantity, orderrows.price FROM orders " + 
        "INNER JOIN orderrows ON orderrows.ordernr=orders.id " +
        "INNER JOIN products ON orderrows.productid = products.id " +
        "INNER JOIN combo ON combo.prodid=orderrows.productid AND combo.color=orderrows.color AND combo.size=orderrows.size " +
        "WHERE orderrows.ordernr =" + myorder + ";";
        var myorderrows = db.Query<Orderrowstwo>(sql).ToList();
        return myorderrows; 
    }


    //SELECT products.name, inhouse.color, inhouse.size, products.price, orderrows.price, inhouse.shelfsid, inhouse.shellnr, inhouse.slot,inhouse.quantity FROM orderrows INNER JOIN products ON orderrows.productid=products.id INNER JOIN combo ON combo.prodid=orderrows.productid AND combo.color=orderrows.color AND combo.size=orderrows.size INNER JOIN inhouse ON combo.prodid=inhouse.prodid AND combo.color=inhouse.color AND combo.size=inhouse.size; 


    static public List<Offer> OfferProducts()
    {
        var sql="SELECT combo.prodid, products.name, products.price, combo.color, GROUP_CONCAT(combo.size SEPARATOR ';') AS size FROM products INNER JOIN combo ON products.id=combo.prodid GROUP BY combo.color; ";
        var offered = db.Query<Offer>(sql).ToList();
        return offered;
    }

    // SELECT orders.odate, customers.name, products.name, orderrows.productid, orderrows.color, orderrows.size, orderrows.quantity, orderrows.price FROM `orders` INNER JOIN orderrows ON orderrows.ordernr = orders.id INNER JOIN customers ON customers.id = orders.customerid INNER JOIN products ON products.id = orderrows.productid WHERE orders.status='delivered';


    //Insert a new order (and the corresponding orderrows)

    static public int GetOrderId(int custid)
    {
        var sql = "SELECT MAX(id), odate, status FROM orders " + "WHERE orders.customerid=" + custid + ";";
        int myorders = db.ExecuteScalar<int>(sql);
        return myorders;
    }

    static public void NewOrder(int custid, Ostat stat)
    {
        // create an order for the customer
        
        var sql="INSERT INTO orders (customerid, odate, status) VALUES (@customerid, NOW(), @status);";
        var myorder = new { customerid = custid, odate = "1900-01-01", status="recieved" };
        var rowsAffected = db.Execute(sql, myorder);

        // var sql="INSERT INTO  orders (customerid, odate, stat) VALUES (\'" +custid +"\', NOW(), \', \'"+ stat "\');" +;

        // Obtain the latest order id for the customer

        int orderid = GetOrderId(custid);

        var sql2="INSERT INTO orderrows (ordernr, productid, color, size, quantity,price, delivquant)"
                + " VALUES (" + orderid +", @productid, @color, @size, @quantity, @price,"+ 0 + ");";
        
        foreach (Orderrows a in Service.myOrderrows)
        {
             var myorderrows = new { ordernr = orderid, productid=a.productid, color= a.color, size=a.size,  quantity= a.quantity, price=a.price, delivquant=0};
             db.Execute(sql2, myorderrows);
        }

    } 

    static public List<Orders> ProcessMyOrders()
    {
        var sql = "SELECT orders.id, orders.customerid, orders.odate, orders.status FROM orders INNER JOIN orderrows ON orderrows.ordernr=orders.id WHERE orderrows.delivquant < orderrows.quantity GROUP BY orders.id";
        var myorders = db.Query<Orders>(sql).ToList();
        return myorders; 
    }

        // "SELECT orders.id, products.name, combo.size, combo.color, orders.odate, orders.status, orderrows.quantity, orderrows.delivquant FROM orders " + 
        // "INNER JOIN orderrows ON orderrows.ordernr=orders.id " +
        // "INNER JOIN products ON orderrows.productid = products.id " +
        // "INNER JOIN customers ON orders.customerid = " +
        // "WHERE orderrows.delivquant < orderrows.quantity ;"

    static public List<ReportStock> StockItems(int myorder, int pid, string cid, string sid)
    {
        var sql = "SELECT products.id, orderrows.ordernr, combo.color, combo.size, products.name, inhouse.shellnr, inhouse.slot, orders.status, orderrows.quantity AS OQ, inhouse.quantity AS IQ " + 
        "FROM orders INNER JOIN orderrows ON orderrows.ordernr=orders.id " +
        "INNER JOIN products ON orderrows.productid = products.id "+
        "INNER JOIN combo ON combo.prodid=orderrows.productid AND combo.color=orderrows.color AND combo.size=orderrows.size "+
        "INNER JOIN inhouse ON combo.prodid=inhouse.prodid AND combo.color = inhouse.color AND combo.size=inhouse.size "+
        "WHERE AND ((inhouse.quantity - orderrows.quantity) > inhouse.orderpoint) AND (orderrows.ordernr=" +myorder +") AND "+pid +"=products.id AND combo.color="+cid+" AND color.size="+sid+ ");";
        var myorderrows = db.Query<ReportStock>(sql).ToList();
        return myorderrows; 
    }

    static public List<ReportStock> LowStockItems(int myorder)
    {
        var sql = "SELECT products.id, orderrows.ordernr, combo.color, combo.size, products.name, inhouse.shellnr, inhouse.slot, orders.status, orderrows.quantity, inhouse.quantity " + 
        "FROM orders INNER JOIN orderrows ON orderrows.ordernr=orders.id " +
        "INNER JOIN products ON orderrows.productid = products.id "+
        "INNER JOIN combo ON combo.prodid=orderrows.productid AND combo.color=orderrows.color AND combo.size=orderrows.size "+
        "INNER JOIN inhouse ON combo.prodid=inhouse.prodid AND combo.color = inhouse.color AND combo.size=inhouse.size "+
        "WHERE (((inhouse.quantity - orderrows.quantity) < (inhouse.orderpoint+1)) AND ((inhouse.quantity - orderrows.quantity)>0)) AND (orderrows.ordernr=" +myorder +" );";
        var myorderrows = db.Query<ReportStock>(sql).ToList();
        return myorderrows; 
    }

    static public List<Orderrowstwo> ProblemStockItems(int myorder)
    {
        var sql = "SELECT products.id, orderrows.ordernr, combo.color, combo.size, products.name, inhouse.shellnr, inhouse.slot, orders.status, orderrows.quantity, inhouse.quantity " + 
        "FROM orders INNER JOIN orderrows ON orderrows.ordernr=orders.id " +
        "INNER JOIN products ON orderrows.productid = products.id "+
        "INNER JOIN combo ON combo.prodid=orderrows.productid AND combo.color=orderrows.color AND combo.size=orderrows.size "+
        "INNER JOIN inhouse ON combo.prodid=inhouse.prodid AND combo.color = inhouse.color AND combo.size=inhouse.size "+
        "WHERE (((inhouse.quantity - orderrows.quantity) < (inhouse.orderpoint+1)) AND ((inhouse.quantity - orderrows.quantity)>0)) AND (orderrows.ordernr=" +myorder +" );";
        var myorderrows = db.Query<Orderrowstwo>(sql).ToList();
        return myorderrows; 
    }static public List<Orderrowstwo> BelowStockItems(int myorder)
    {
        var sql = "SELECT products.id, orderrows.ordernr, combo.color, combo.size, products.name, inhouse.shellnr, inhouse.slot, orders.status, orderrows.quantity, inhouse.quantity " + 
        "FROM orders INNER JOIN orderrows ON orderrows.ordernr=orders.id " +
        "INNER JOIN products ON orderrows.productid = products.id "+
        "INNER JOIN combo ON combo.prodid=orderrows.productid AND combo.color=orderrows.color AND combo.size=orderrows.size "+
        "INNER JOIN inhouse ON combo.prodid=inhouse.prodid AND combo.color = inhouse.color AND combo.size=inhouse.size "+
        "WHERE ((inhouse.quantity - orderrows.quantity) < 0;) AND (orderrows.ordernr=" +myorder +" );";
        var myorderrows = db.Query<Orderrowstwo>(sql).ToList();
        return myorderrows; 
    }

    // SELECT products.id, combo.color, combo.size, products.name, SUM(inhouse.quantity)-SUM(orderrows.quantity) FROM orders INNER JOIN orderrows ON orderrows.ordernr=orders.id INNER JOIN products ON orderrows.productid = products.id INNER JOIN combo ON combo.prodid=orderrows.productid AND combo.color=orderrows.color AND combo.size=orderrows.size INNER JOIN inhouse ON combo.prodid=inhouse.prodid AND combo.color = inhouse.color AND combo.size=inhouse.size; 

    // UPDATE orderrows INNER JOIN products ON orderrows.productid=products.id SET orderrows.price = products.price WHERE orderrows.ordernr=9; 

}