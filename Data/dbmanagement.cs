using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using BlazorServerApp.Data;
using Mysqlx.Crud;


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

}