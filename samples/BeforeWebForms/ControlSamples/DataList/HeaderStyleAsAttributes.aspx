<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HeaderStyleAsAttributes.aspx.cs" Inherits="BeforeWebForms.ControlSamples.DataList.HeaderStyleAsAttributes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>DataList TableItemStyle</h2>

    <div>
        Other usage samples: <a href="Default.aspx">Simple Layout Sample</a>  <a href="FlowLayout.aspx">FlowLayout Sample</a>  <a href="StyleAttributes.aspx">Styles</a>
    </div>

    <p>
        Here is a simple datalist bound to a collection of widgets.  We're testing and showing the various
		style attributes that can be set on the DataList
    </p>

    <asp:DataList ID="simpleDataList"
        runat="server"
        RepeatColumns="2"
        ToolTip="This is my tooltip"
        AccessKey="S"
        BackColor="Gray"
        BorderStyle="Solid"
        BorderWidth="2px"
        BorderColor="Firebrick"
        Font-Bold="true"
        GridLines="Vertical"
        UseAccessibleHeader="true"
        ItemType="SharedSampleObjects.Models.Widget">
        <HeaderStyle BackColor="#0000ff" BorderStyle="Solid" BorderColor="Black" BorderWidth="2" />
        <HeaderTemplate>HeaderTemplate</HeaderTemplate>
        <FooterTemplate>End of Line</FooterTemplate>
        <ItemTemplate><%# Item.Name %></ItemTemplate>
    </asp:DataList>

</asp:Content>
