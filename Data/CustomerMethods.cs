namespace BlazorServerApp.Data
{
class CustomerMethods
{
    

    
    
    static public string PrintCustomer(List<Customer> customers)
    {
        String outhtml ="<font size=\"1\">";
        outhtml += "<br>------------------------------------------------<br>";
        outhtml += "               Customers<br>";
        outhtml += "------------------------------------------------<br>";
        foreach( Customer a in customers)
        {
            outhtml += $" Customer Id {a.customerid} <br>";
            outhtml += $" Customer adress {a.adress} <br>";
            outhtml += $" Customer phone {a.phone} <br>";
            outhtml += $" Customer email: {a.email} <br>";
            outhtml += "------------------------------------------------<br>";
        }
        outhtml +="</font>";
        
        return outhtml;
    }

    static public string PrintCustomerdb()
    {
        List<Customer> customers =  Dbmethods.PrintCust();
        String outhtml ="<font size=\"1\">";
        outhtml += "<br>------------------------------------------------<br>";
        outhtml += "               Customers<br>";
        outhtml += "------------------------------------------------<br>";
        foreach(Customer a in customers)
        {
            outhtml += $" Customer Id {a.customerid} <br>";
            outhtml += $" Customer name {a.name} <br>";
            outhtml += $" Customer adress {a.adress} <br>";
            outhtml += $" Customer phone {a.phone} <br>";
            outhtml += $" Customer email: {a.email} <br>";
            outhtml += "------------------------------------------------<br>";
        }
        outhtml +="</font>";
        
        return outhtml;
    }
}
}