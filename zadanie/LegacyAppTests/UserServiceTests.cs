using LegacyApp;

namespace LegacyAppTests;

public class UserServiceTests
{
    [Fact]
    public void Add_User_Should_Return_False_When_No_At_Or_Dot()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Smith";
        DateTime birthDate = new DateTime(2000, 3, 3);
        int clientId = 1;
        string email = "hello";
        var service = new UserService();
        
        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        //Assert
        Assert.False(result);
    }

    [Fact]
    public void Add_User_Should_Return_False_When_Name_Empty()
    {
        //Arrange
        string firstName = "";
        string lastName = null;
        DateTime birthDate = new DateTime(2000, 3, 3);
        int clientId = 1;
        string email = "smith@gmail.com";
        var service = new UserService();
        
        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        //Assert
        Assert.False(result); 
    }
    
    [Fact]
    public void Add_User_Should_Return_False_When_Age_Below_21()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Smith";
        DateTime birthDate = new DateTime(2020, 3, 3);
        int clientId = 1;
        string email = "smith@gmail.com";
        var service = new UserService();
        
        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        //Assert
        Assert.False(result); 
    }
    
    [Fact]
    public void Add_User_Should_Return_False_When_User_Has_Credit_Limit_Or_Below_500()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Smith";
        DateTime birthDate = new DateTime(2020, 3, 3);
        int clientId = 1;
        string email = "smith@gmail.com";
        var user = new User();
        user.HasCreditLimit = true;
        var service = new UserService();
        
        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        //Assert
        Assert.False(result); 
    }
    
    
}