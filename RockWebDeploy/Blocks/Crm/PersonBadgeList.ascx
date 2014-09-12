﻿<%@ control language="C#" autoeventwireup="true" inherits="RockWeb.Blocks.Crm.PersonBadgeList, RockWeb" %>

<asp:UpdatePanel ID="upPersonBadge" runat="server">
    <ContentTemplate>

        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        
        <div class="panel panel-block">
            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-shield"></i> Person Profile Badge List</h1>
            </div>
            <div class="panel-body">

                <div class="grid grid-panel">
                    <Rock:Grid ID="gPersonBadge" runat="server" AllowSorting="false" OnRowSelected="gPersonBadge_Edit">
                        <Columns>
                            <Rock:ReorderField />
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                            <Rock:SecurityField TitleField="Name" />
                            <Rock:DeleteField OnClick="gPersonBadge_Delete" />
                        </Columns>
                    </Rock:Grid>
                </div>

            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
