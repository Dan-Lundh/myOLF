namespace BlazorServerApp.Data;
public class Customer
{
    public int customerid {set; get;}
    public string name {set; get;}
    public string adress {set; get;}
    public string email {set; get;}    
    public string phone {set; get;} 
    public Customer()
    {
        customerid = 0; 
        name = "";
        adress = "";
        email = "";
        phone = "";
    }

    public Customer(int cId, string fname, string lname, string sId, string adr, string epost, string phonenr)
    {
        customerid = cId;
        name = lname;
        adress = adr;
        email = epost;
        phone = phonenr;
    }
}

