$(function () {
    var connect = $.connection.wolHub;



    // Create a dummy client method
    $.connection.wolHub.client.dm = function() {
        
    };

    $.connection.hub.start().done(function () {
        connect.server.connect();       
        GetPCList();
        console.log("Connected Done");
    });

    $(document).ready(function () {
        //console.log("3");
        //connect.server.connect();
    });

    $("#rescanbutton").click(function () {       
        GetPCList();        
    });

    $("#addnewrowbutton").click(function () {
        AddNewRow();
    });

    $("#updatecurrentbutton").click(function () {
        UpdateCurrentPCs();
    });

    function AddNewRow() {
        var tablebody = $("#issuetable tbody");
        tablebody.prepend(GetEmptyRow());
        EditableText();
        UpdateButtonEvent();
    }
    
    function GetPCList() {
        connect.server.getPCList(false).done(function (pcList) {
            console.log("getPCList END");
            UpdateAllTable(pcList);
            $("#rescanbutton").prop('disabled', false);
            DateOfLastUpdate();
        });
    }

    function UpdateCurrentPCs() {
        var rows = $("tr[id]");
        var ips = [];
        rows.each(function (i) {
            ips.push(rows.eq(i).find("td").eq(2).text());            
        });

        connect.server.updateStatus(ips).done(function (data) {
            data.forEach(function (item) {
                var row = GetRowByIp(item.Ip);
                row.find("td").eq(5).text(item.IsOnline);
                if (item.IsOnline) {
                    row.find("td").eq(5).css("background-color", "green");
                }
                else {
                    row.find("td").eq(5).css("background-color", "red");
                }
            });
            DateOfLastUpdate();
        });
    }

    function EditableText() {
        document.querySelectorAll("table tr td[editable]").forEach(function (node) {
            node.ondblclick = function () {
                var val = this.innerHTML;
                var input = document.createElement("input");
                input.value = val;
                input.onblur = function () {
                    var val = this.value;
                    this.parentNode.innerHTML = val;
                }
                this.innerHTML = "";
                this.appendChild(input);
                input.focus();
            }
        });
    }

    function UpdateAllTable(pcList) {
        var tablebody = $("#issuetable tbody");
        var rows = tablebody.find("tr");
        rows.remove();

        pcList.forEach(e => tablebody.append(GetFillRow(e)));

        EditableText();
        UpdateButtonEvent();
    }

    $(document).ready(function () {
    });

    function GetEmptyRow() {
        return "<tr id='" + 0 + "'><td>" + 0 + "</td><td></td><td editable>" + "0.0.0.0" + "</td><td editable>" + "MAC" + "</td><td editable></td><td style = 'background-color: red;'>" + "false" + "</td><td><button saveWOLbutton class='btn btn-default' type='button'><img src='Content/img/save.svg'/></button><button WOLbutton class='btn btn-default' type='button'><img src='Content/img/lightbulb.svg'/></button><button deletebutton class='btn btn-default' type='button'><img src='Content/img/bucket.svg'/></button><button updatebutton class='btn btn-default' type='button'><img src='Content/img/arrow-counterclockwise.svg'/></button></td></tr>";
    }

    function GetFillRow(e) {
        var statusColor;
        if (e.IsOnline) {
            statusColor = "style = 'background-color: green;'";
        }
        else {
            statusColor ="style = 'background-color: red;'";
        }
        return "<tr id='" + e.Id + "'><td>" + e.Id + "</td><td>" + e.AccountLogin + "</td><td editable>" + e.Ip + "</td><td editable>" + e.MAC + "</td><td editable>" + e.PCName + "</td><td " + statusColor + ">" + e.IsOnline + "</td><td><button saveWOLbutton class='btn btn-default' type='button'><img src='Content/img/save.svg'/></button><button WOLbutton class='btn btn-default' type='button'><img src='Content/img/lightbulb.svg'/></button><button deletebutton class='btn btn-default' type='button'><img src='Content/img/bucket.svg'/></button><button updatebutton class='btn btn-default' type='button'><img src='Content/img/arrow-counterclockwise.svg'/></button></td></tr>";
    }

    function GetRowByIp(ip) {

        var rows = $("tr[id]");
        var res;
        rows.each(function (i) {
            if (rows.eq(i).find("td").eq(2).text() == ip) {                
                res = rows.eq(i);
            }   
        });

        return res;
    }

    function DateOfLastUpdate() {
        var currentdate = new Date();
        $("#lastupdateinfo").find("span").text("Последнее обновление : " + currentdate.toLocaleTimeString());
    }

    function UpdateButtonEvent() {
        $("button[deletebutton]").click(function () {
            var row = $(this).parent().parent().find("td");
            var id = row.eq(0).text();

            connect.server.delete(id).done(function (id) {
                var rows = $("td:contains('" + id + "')");

                rows.each(function (index) {
                    if (row.eq(0).text() == id) {
                        row.remove();
                    }
                });
            });
        });

        $("button[updatebutton]").click(function () {
            var row = $(this).parent().parent().find("td");

            var ip = row.eq(2).text();            
            
            connect.server.updateStatusByIp(ip).done(function (ret) {
                
                var row = GetRowByIp(ret.ip).find("td");
                
                row.eq(5).text(ret.status);

                if (ret.status) {
                    row.eq(5).css("background-color", "green");
                }
                else {
                    row.eq(5).css("background-color", "red");
                }
            });
        });        

        $("button[WOLbutton]").click(function () {
            var row = $(this).parent().parent().find("td");
            var id = row.eq(0).text();
            var ip = row.eq(1).text();
            var login = row.eq(2).text();
            var mac = row.eq(3).text();
            var name = row.eq(4).text();

            console.log(id);
            console.log(ip);
            console.log(login);
            console.log(mac);
            console.log(name);

            connect.server.wOL(mac);
        });

        $("button[saveWOLbutton]").click(function () {
            var row = $(this).parent().parent().find("td");
            var id = row.eq(0);
            var login = row.eq(1);
            var ip = row.eq(2);
            var mac = row.eq(3);
            var name = row.eq(4);

            connect.server.savePCNI(id.text(), mac.text(), ip.text(), name.text(), login.text()).done(function (data) {
                var rows = $("td:contains('" + data.Ip + "')");

                rows.each(function (index) {
                    console.log(index);
                    if (row.eq(2).text() == data.Ip) {
                        row.eq(0).text(data.Id);
                    }
                });          
            });
        });
    }
});


