<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Database.aspx.cs" Inherits="WebApplication3.Database" %>

<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link rel="stylesheet" href="data.css"/>
    <link rel="stylesheet" href="dataB.css"/>

</head>
<body>
    <header id="site-header">
    
        <h1 class="site-title"> Admin Area</h1>
        <h2 class="site-description">In that area you can manage employees and check the time scheduling</h2>
</header>
   <form id="form1" runat="server">
    
    <section id="grid1" class="grid-group1">
        
        <p>That table show the employees in data base.</p>
        

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="iduser"
        CssClass="mydatagrid" PagerStyle-CssClass="pager" HorizontalAlign="Center"
        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"  
        OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" 
        OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No records has been added.">
        <Columns>
            <asp:TemplateField HeaderText="UserId" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblUserid" runat="server" Text='<%# Eval("iduser") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtUserid" runat="server" Text='<%# Eval("iduser") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Name" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Surname" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblSurname" runat="server" Text='<%# Eval("Surname") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtSurname" runat="server" Text='<%# Eval("Surname") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="role" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblRole" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtRole" runat="server" Text='<%# Eval("Role") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Path" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblPath" runat="server" Text='<%# Eval("Path") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPath" runat="server" Text='<%# Eval("Path") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>




            <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true"
                ItemStyle-Width="150" />
        </Columns>
    </asp:GridView>

        <br />
        <p>Insert a new employee.</p>
    <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
        <tr>
            <td style="width: 150px">
                UserID:<br />
                <asp:TextBox ID="txtUserId" runat="server" Width="140" />
            </td>
            <td style="width: 150px">
                Name:<br />
                <asp:TextBox ID="txtname" runat="server" Width="140" />
            </td>
             <td style="width: 150px">
                Surname:<br />
                <asp:TextBox ID="txtSurname" runat="server" Width="140" style="margin-bottom: 2px" />
            </td>
             <td style="width: 150px">
                Role:<br />
                <asp:TextBox ID="txtRole" runat="server" Width="140" />
            </td>
             <td style="width: 150px">
                Path:<br />
                <asp:TextBox ID="txtPath" runat="server" Width="140" />
            </td>
            <td style="width: 100px">

                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="Insert" CssClass="button1" />
            </td>
        </tr>
    </table>
        </section>

        <br />
 
       <section id="grid2" >
           <p>This table shows the employee schedule</p>
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" DataKeyNames="iduser"
             CssClass="mydatagrid" PagerStyle-CssClass="pager" HorizontalAlign="Center"
        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"  
        EmptyDataText="No records has been added." >
        <Columns>
            <asp:TemplateField HeaderText="ID" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtID" runat="server" Text='<%# Eval("id") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="UserID" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("iduser") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtUserID" runat="server" Text='<%# Eval("iduser") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Entry Time" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblentrytime" runat="server" Text='<%# Eval("entrytime") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEntryTime" runat="server" Text='<%# Eval("entryetime") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Exit Time" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblexitime" runat="server" Text='<%# Eval("exitime") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtexitime" runat="server" Text='<%# Eval("exitime") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Date" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("currDate") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtDate" runat="server" Text='<%# Eval("currDate") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


        </Columns>
    </asp:GridView>
       </section>

       <section id="grid3">
         <br />
        <p>In that section you can find emploee schedul searching by id</p>

        

        <br />
        <br />

        

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
           
        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" />

        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="false" DataKeyNames="iduser"
             CssClass="mydatagrid" PagerStyle-CssClass="pager" 
        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"  HorizontalAlign="Center"
        EmptyDataText="No records has been found."  >
        <Columns>
            


            <asp:TemplateField HeaderText="UserID" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("iduser") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtUserID" runat="server" Text='<%# Eval("iduser") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Entry Time" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblentrytime" runat="server" Text='<%# Eval("entrytime") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEntryTime" runat="server" Text='<%# Eval("entryetime") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Exit Time" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblexitime" runat="server" Text='<%# Eval("exitime") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtexitime" runat="server" Text='<%# Eval("exitime") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Date" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("currDate") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtDate" runat="server" Text='<%# Eval("currDate") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


        </Columns>
    </asp:GridView>

       </section>

       


    </form>
</body>
</html>

