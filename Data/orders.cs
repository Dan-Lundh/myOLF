using System.Security.Permissions;
using System;


namespace BlazorServerApp.Data;
public class Orders
{
    public int id {set; get;} // order id
    public int customerid {set;get;} // customer id
    public DateTime odate {set;get;} // date and time

    public Ostat status; // enum('recieved','split delivery','deliver one go','invoice sent','delivered')


    
}


public enum Ostat
{
    
    recieved,  
    split_delivery,
    deliver_one_go,
    invoice_sent,
    delivered
}

public class Orderrows
{
    // `id`, `ordernr (FK order)`, `productid`, 'color', 'size', `quantity`, `price`, `delivquant`

    public int id {set; get;}
    public int ordernr {set;get;}
    public int productid {set;get;}
    public string color {get;set;} ="";
    public string size {get;set;} ="";
    public int quantity {set;get;}
    public float price {set;get;}
    public int delivquant {set;get;}
}

// SELECT products.id, orderrows.ordernr, combo.color, combo.size, products.name, inhouse.shellnr, 
// inhouse.slot, orders.status, orderrows.quantity, inhouse.quantity

public class ReportStock
{
    public int id {set; get;}
    public int ordernr {set;get;}
    public string color {get;set;} ="";
    public string size {get;set;} ="";
    public string name {set; get;} ="";
    public int shellnr {get;set;}
    public int slot {get;set;}
    public int orderquant {get;set;}
    public int stockquant {get;set;}

}


public class Orderrowstwo {
    public string name {set; get;} ="";
    public DateTime odate {set; get;}
    public Ostat status {set; get;}
    public int quantity {set; get;}
    public float price {set; get;}
    public int id {set;get;} 
    public string color {set;get;} ="";
    public string size {set;get;} ="";
}

// inhouse status
//ENUM('Expected_delivery','Out_of_date','Return_to_supplier','None','Product_failure','Returned')

public class InHouse {
    // Warehouse details, where to find the products and how many we have
    public int id {get; set;}
    public string color {get; set;} ="";
    public int orderpoint {get; set;}
    public int prodid {get; set;}
    public Inhousstatus prodstatus;
    public int quantity {get; set;}
    public int Shelfsid {get; set;}
    public int shellnr {get; set;}
    public string size {get; set;} ="";
    public int slot {get; set;}
}

public enum Inhousstatus
{
    Expected_delivery,
    Out_of_date,
    Return_to_supplier,
    None,
    Product_failure,
    Returned
}


public class Offer {
    public int prodid {set;get;} 
    public string name {set;get;} ="";
    public float price {set;get;} 
    public string color {set;get;} ="";
    public string size {set;get;} =""; 
}

public class TmpClass {
    public int Id {get;set;}
    public string size {get;set;} ="";
}