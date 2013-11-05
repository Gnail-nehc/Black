$(document).ready(function () {
	LoadTypes();
	$("#ControlProperty_Type").change(function () {
		$("#ControlProperty_Property").empty();
		var controlType = $("#ControlProperty_Type").val();
		LoadProperties(controlType);
	});
});
	function LoadTypes() {
		$.ajax({
			type: "GET",
			url: "Create/LoadTypes",
			timeout: 30000,
			dataType: "json",
			cache: false,
			success: function (msg) {
				var data = msg;
				for (var i = 0; i < data.length; i++) {
					$("#ControlProperty_Type").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
				}
			},
			error: function (xhr) {
				alert(xhr.responseText);
			}
		});
	}

	function LoadProperties(controlType) {
		$.ajax({
			type: "GET",
			url: "Create/LoadProperties",
			timeout: 30000,
			dataType: "json",
			data: { controlType: controlType },
			success: function (json) {
				var data = json;
				for (var i = 0; i < data.length; i++) {
					$("#ControlProperty_Property").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
				}
			},
			error: function (xhr) {
				alert(xhr.responseText);
			}
		});
	}

