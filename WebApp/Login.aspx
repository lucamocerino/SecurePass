<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication3.Login" %>

<!DOCTYPE html>
<html >
<head>
  <meta charset="UTF-8">
  <title>Login form shake effect</title>
  
  
     <link rel='stylesheet prefetch' href='http://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css'>

      <link rel="stylesheet" href="css/style.css">

  
</head>

<body>
    <form id="form1" runat="server">
  <div class="login-form">

     <h1>SecurePass</h1>
       <h2>Use an administrator account to log in</h2>

      <asp:Label ID="Label1"  runat="server"  ForeColor="Red" CssClass="isa_info" Text =""></asp:Label>

     <div class="form-group ">

      <asp:TextBox ID="textUsername" CssClass="form-control" runat="server" Width="285px"></asp:TextBox>
     </div>
        
       <div class="form-group log-status">
          
       <asp:TextBox ID="textPassword" TextMode="Password" CssClass="form-control" runat ="server" Width="284px"></asp:TextBox>
      </div>
      

      <asp:Button ID="Button1" CssClass="log-btn" runat="server"  Text="Login" OnClick="Login_Click" Height="31px" Width="302px" />

     
   </div>
    
     
 

        </form>
</body>
</html>
