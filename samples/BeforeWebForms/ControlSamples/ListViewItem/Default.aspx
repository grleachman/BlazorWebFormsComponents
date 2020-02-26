<%@ Page Title="ListView Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BeforeWebForms.ControlSamples.ListViewItem.Default" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">

    <h2>ListView control homepage</h2>

    <div>
        <a href="Default.aspx">Default</a>
    </div>

    <p>Here is a simple listview demonstrating the use of ListViewItem.</p>

    <asp:ListView ID="simpleListView"
        runat="server"
        Enabled="true"
        InsertItemPosition="LastItem"
        ItemInserting="simpleListView_ItemInserting"
        ItemType="SharedSampleObjects.Models.Widget">
        <LayoutTemplate>
            <table>
                <thead>
                    <tr>
                        <td>Id</td>
                        <td>Name</td>
                        <td>Price</td>
                        <td>Last Update</td>
                    </tr>
                </thead>
                <tbody>
                    <tr runat="server" id="itemPlaceHolder"></tr>
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td><asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' /></td>
                <td><asp:Label ID="NameLabel" runat="server" Text='<%# Eval("Name") %>' /></td>
                <td><asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("Price","{0:c}") %>' /></td>
                <td><asp:Label ID="LastUpdateLabel" runat="server" Text='<%# Eval("LastUpdate","{0:d}") %>' /></td>
            </tr>
        </ItemTemplate>
        <InsertItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="IdTextBox" runat="server" Text='<%#Bind("Id") %>' MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox ID="NameTextBox" runat="server" Text='<%#Bind("Name") %>' MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox ID="PriceTextBox" runat="server" Text='<%#Bind("Price","c") %>' MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox ID="LastUpdateTextBox" runat="server" Text='<%#Bind("LastUpdate","d") %>' MaxLength="50" />
                </td>
            </tr>
            <tr class="bgcolor" runat="server">
            <td colspan="4">
              <asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" Text="Add" />
              <asp:LinkButton ID="CancelInsertButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
          </tr>>
        </InsertItemTemplate>
        <EmptyDataTemplate>
            <tr>
                <td colspan="4">No widgets available</td>
            </tr>
        </EmptyDataTemplate>
        <EmptyItemTemplate></EmptyItemTemplate>
    </asp:ListView>
    <code></code>
</asp:Content>
