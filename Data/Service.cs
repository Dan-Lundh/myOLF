namespace BlazorServerApp.Data;

public class Service
{
    
    public static List<Customer> customers {get; set;} = new List<Customer>();

    public static List<Orders> myOrder = new List<Orders>();

    public static List<Orderrows> myOrderrows = new List<Orderrows>();
    
    public static Customer currentCustomer = new Customer();


    //Customer mycust = new Customer(1,"Anders","Andersson", "19700304-1345","Storgatan 42, 43431 Kungsbacka","anders@hotmail.com","034-121121");
}

