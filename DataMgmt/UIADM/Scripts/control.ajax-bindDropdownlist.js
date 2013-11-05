$(document).ready(function () {
	var url_lt = $("#url_lt")[0].value;
	var url_lp = $("#url_lp")[0].value;
	LoadTypes(url_lt);
    $("#SearchedConditionEntity_ControlType").change(function () {
        $("#SearchedConditionEntity_ControlProperty").empty();
        var controlType = $("#SearchedConditionEntity_ControlType").val();
        LoadProperties(url_lp, controlType);
    });
});
	function LoadTypes(url) {
        $.ajax({
            type: "GET",
            url: url,
            timeout: 30000,
            dataType: "json",
            cache :false, 
            success: function (msg) {
                var data = msg;
                for (var i = 0; i < data.length; i++) {
                    $("#SearchedConditionEntity_ControlType").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                }
            },
            error: function (xhr) {
            	alert(xhr.responseText);
            }
        });
    }

    function LoadProperties(url,controlType) {
        $.ajax({
            type: "GET",
            url: url,
            timeout: 30000,
            dataType: "json",
            data: { controlType: controlType },
            success: function (json) {
                var data = json;
                for (var i = 0; i < data.length; i++) {
                    $("#SearchedConditionEntity_ControlProperty").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                }
            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
    }

 