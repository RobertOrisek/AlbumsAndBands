var host = "https://localhost:";
var port = "44324/";
var albumsEndpoint = "api/albumi/";
var bandsEndpoint = "api/bendovi/";
var loginEndpoint = "api/authentication/login";
var searchEndpoint = "api/albumi/pretraga/"
var registerEndpoint = "api/authentication/register";
var formAction = "Create";
var editingId;
var jwt_token;

function loadPage() {
	loadAlbums();
	loadBandsForDropdown();
}
//------------------------------------------------------------------------------
// LOGIN & REGISTER
//------------------------------------------------------------------------------
function showLogin() {
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "block";
	document.getElementById("registerFormDiv").style.display = "none";
	document.getElementById("logout").style.display = "none";
}

function showRegistration() {
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "none";
	document.getElementById("registerFormDiv").style.display = "block";
	document.getElementById("logout").style.display = "none";
}

function validateRegisterForm(username, email, password, confirmPassword) {
	if (username.length === 0) {
		alert("Username field can not be empty.");
		return false;
	} else if (email.length === 0) {
		alert("Email field can not be empty.");
		return false;
	} else if (password.length === 0) {
		alert("Password field can not be empty.");
		return false;
	} else if (confirmPassword.length === 0) {
		alert("Confirm password field can not be empty.");
		return false;
	} else if (password !== confirmPassword) {
		alert("Password value and confirm password value should match.");
		return false;
	}
	return true;
}

function validateLoginForm(username, password) {
	if (username.length === 0) {
		alert("Username field can not be empty.");
		return false;
	} else if (password.length === 0) {
		alert("Password field can not be empty.");
		return false;
	}
	return true;
}

function registerUser() {
	var username = document.getElementById("usernameRegister").value;
	var email = document.getElementById("emailRegister").value;
	var password = document.getElementById("passwordRegister").value;
	var confirmPassword = document.getElementById("confirmPasswordRegister").value;

	if (validateRegisterForm(username, email, password, confirmPassword)) {
		var url = host + port + registerEndpoint;
		var sendData = { "Username": username, "Email": email, "Password": password };
		fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
			.then((response) => {
				if (response.status === 200) {
					document.getElementById("registerForm").reset();
					console.log("Successful registration");
					alert("Successful registration");
					showLogin();
				} else {
					console.log("Error occured with code " + response.status);
					console.log(response);
					alert("Error occured!");
					response.text().then(text => { console.log(text); })
				}
			})
			.catch(error => console.log(error));
	}
	return false;
}

function loginUser() {
	var username = document.getElementById("usernameLogin").value;
	var password = document.getElementById("passwordLogin").value;

	if (validateLoginForm(username, password)) {
		var url = host + port + loginEndpoint;
		var sendData = { "Username": username, "Password": password };
		fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
			.then((response) => {
				if (response.status === 200) {
					document.getElementById("loginForm").reset();
					console.log("Successful login");
					alert("Successful login");
					response.json().then(function (data) {
						console.log(data);
						document.getElementById("userLogged").innerHTML = "Prijavleni korisnik: <i>" + data.username + "<i/>.";
						document.getElementById("logout").style.display = "block";
						
						document.getElementById("btnLogin").style.display = "none";
						document.getElementById("btnRegister").style.display = "none";
						document.getElementById("loginFormDiv").style.display = "none";
						document.getElementById("searchDiv").style.display = "block";
						jwt_token = data.token;
						refreshPage();
					});
				} else {
					console.log("Error occured with code " + response.status);
					console.log(response);
					alert("Error occured!");
					response.text().then(text => { console.log(text); })
				}
			})
			.catch(error => console.log(error));
	}
	return false;
};

function logout() {
	jwt_token = undefined;
	document.getElementById("data").innerHTML = "";
	document.getElementById("userLogged").innerHTML = "Korisnik nije prijavljen na sistem";
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "block";
	document.getElementById("registerFormDiv").style.display = "none";
	document.getElementById("logout").style.display = "none";
	document.getElementById("btnLogin").style.display = "initial";
	document.getElementById("btnRegister").style.display = "initial";
	document.getElementById("searchDiv").style.display = "none";
	loadAlbums();
};
//------------------------------------------------------------------------------
// ALBUMS
//------------------------------------------------------------------------------
function loadAlbums() {
	document.getElementById("loginFormDiv").style.display = "none";
	document.getElementById("registerFormDiv").style.display = "none";

	var requestUrl = host + port + albumsEndpoint;
	var headers = {};
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}
	console.log(headers);
	fetch(requestUrl, { headers: headers })
		.then((response) => {
			if (response.status === 200) {
				response.json().then(setAlbums);
			} else {
				console.log("Error occured with code " + response.status);
				showError("Error occured while retrieving data.");
			}
		})
		.catch(error => console.log(error));
};

function setAlbums(data) {
	var container = document.getElementById("data");
	container.innerHTML = "";

	console.log(data);

	// ispis naslova
	var div = document.createElement("div");
	var h1 = document.createElement("h1");
	var headingText = document.createTextNode("Albums");
	h1.appendChild(headingText);
	div.appendChild(h1);

	var table = document.createElement("table");
	table.className = "table table-bordered";

	var header = createHeader();
	table.append(header);

	var tableBody = document.createElement("tbody");
	console.log(data);
	for (var i = 0; i < data.length; i++) {
		var row = document.createElement("tr");
		row.appendChild(createTableCell(data[i].name));
		row.appendChild(createTableCell(data[i].publishingYear));
		row.appendChild(createTableCell(data[i].genre));
		row.appendChild(createTableCell(data[i].bandName));
		row.appendChild(createTableCell(data[i].copiesSold));

		if (jwt_token) {
			var buttonDelete = document.createElement("button");
			buttonDelete.name = data[i].id.toString();
			buttonDelete.addEventListener("click", deleteAlbum);
			buttonDelete.className = "btn btn-danger";
			var buttonDeleteText = document.createTextNode("Delete");
			buttonDelete.appendChild(buttonDeleteText);
			var buttonDeleteCell = document.createElement("td");
			buttonDeleteCell.appendChild(buttonDelete);
			row.appendChild(buttonDeleteCell);
			
			var buttonEdit = document.createElement("button");
			buttonEdit.name = data[i].id.toString();
			buttonEdit.addEventListener("click", editAlbum);
			buttonEdit.className = "btn btn-warning";
			var buttonEditText = document.createTextNode("Edit");
			buttonEdit.appendChild(buttonEditText);
			var buttonEditCell = document.createElement("td");
			buttonEditCell.appendChild(buttonEdit);
			row.appendChild(buttonEditCell);
		}
		tableBody.appendChild(row);
	}

	table.appendChild(tableBody);
	div.appendChild(table);

	document.getElementById("formDiv").style.display = "block";
	container.appendChild(div);
};
//------------------------------------------------------------------------------
// BANDS
//------------------------------------------------------------------------------
function loadBandsForDropdown() {
	var requestUrl = host + port + bandsEndpoint;
	console.log("URL zahteva: " + requestUrl);

	var headers = {};
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(requestUrl, {headers: headers})
		.then((response) => {
			if (response.status === 200) {
				response.json().then(setBandsInDropdown);
			} else {
				console.log("Error occured with code " + response.status);
			}
		})
		.catch(error => console.log(error));
};

function setBandsInDropdown(data) {
	var dropdown = document.getElementById("albumBand");
	dropdown.innerHTML = "";
	for (var i = 0; i < data.length; i++) {
		var option = document.createElement("option");
		option.value = data[i].id;
		var text = document.createTextNode(data[i].name);
		option.appendChild(text);
		dropdown.appendChild(option);
	}
};
//------------------------------------------------------------------------------
// FORMS
//------------------------------------------------------------------------------
function submitSearchForm(){
	var maxPublishingYear = document.getElementById("maxPublishingYear").value;
	var minPublishingYear = document.getElementById("minPublishingYear").value;
	var sendData = {
		"Minimum": minPublishingYear,
		"Maximum": maxPublishingYear,
	};
	var url = host + port + searchEndpoint;

	fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
		.then((response) => {
			if (response.status === 200) {
				response.json().then(setAlbums);
				document.getElementById("searchForm").reset();
			} else {
				console.log("Error occured with code " + response.status);
				showError("Error occured while submitting form.");
			}
		})
		.catch(error => console.log(error));
	return false;
};

function submitAlbumForm() {

	var albumBand = document.getElementById("albumBand").value;
	var albumName = document.getElementById("albumName").value;
	var albumPublishingYear = document.getElementById("albumPublishingYear").value;
	var albumGenre = document.getElementById("albumGenre").value;
	var albumCopiesSold = document.getElementById("albumCopiesSold").value;
	var httpAction;
	var sendData;
	var url;

	if(formAction === "Create") {
		httpAction = "POST";
		url = host + port + albumsEndpoint;
		sendData = {
			"Name": albumName,
			"PublishingYear": albumPublishingYear,
			"Genre": albumGenre,
			"CopiesSold": albumCopiesSold,
			"BandId": albumBand
		};
	} else {
		httpAction = "PUT";
		url = host + port + albumsEndpoint + editingId.toString();
		sendData = {
			"Id" : editingId,
			"Name": albumName,
			"PublishingYear": albumPublishingYear,
			"Genre": albumGenre,
			"CopiesSold": albumCopiesSold,
			"BandId": albumBand
		};
	}

	console.log("Objekat za slanje:");
	console.log(sendData);

	var headers = { 'Content-Type': 'application/json' };

	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(url, { method: httpAction, headers: headers, body: JSON.stringify(sendData) })
		.then((response) => {
			if (response.status === 200 || response.status === 201) {
				console.log("Successful action");
				refreshPage();
			} else {
				console.log("Error occured with code " + response.status);
				alert("Error occured!");
			}
		})
		.catch(error => console.log(error));
	return false;
}
//------------------------------------------------------------------------------
// OTHER FUNCTIONALITIES
//------------------------------------------------------------------------------
function deleteAlbum() {
	var deleteID = this.name;
	var url = host + port + albumsEndpoint + deleteID.toString();
	var headers = { 'Content-Type': 'application/json' };
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(url, { method: "DELETE", headers: headers})
		.then((response) => {
			if (response.status === 204) {
				console.log("Successful action");
				refreshPage();
			} else {
				console.log("Error occured with code " + response.status);
				alert("Error occured!");
			}
		})
		.catch(error => console.log(error));
};

function editAlbum() {
	var editId = this.name;

	var url = host + port + albumsEndpoint + editId.toString();
	var headers = { 'Content-Type': 'application/json' };
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}
	fetch(url)
		.then((response) => {
			if (response.status === 200) {
				response.json().then(data => {
					console.log("Dobijeni objekat:");
					console.log(data);
					document.getElementById("albumName").value = data.name;
					document.getElementById("albumPublishingYear").value = data.publishingYear;
					document.getElementById("albumGenre").value = data.genre;
					document.getElementById("albumCopiesSold").value = data.copiesSold;
					document.getElementById("albumBand").value = data.bandId;
					editingId = data.id;
					formAction = "Update";
				})
			} else {
				formAction = "Create";
				console.log("Error occured with code " + response.status);
				showError("Error occured while editing.");
			}
		})
		.catch(error => console.log(error));
};

function showError(text) {
	var container = document.getElementById("data");
	container.innerHTML = "";

	var div = document.createElement("div");
	var h1 = document.createElement("h1");
	var errorText = document.createTextNode(text);

	h1.appendChild(errorText);
	div.appendChild(h1);
	container.append(div);
}

function refreshPage() {
	document.getElementById("albumsForm").reset();
	document.getElementById("searchForm").reset();
	document.getElementById("data").innerHTML = "";
	loadAlbums();
};

function createHeader() {
	var thead = document.createElement("thead");
	var row = document.createElement("tr");
	thead.style.backgroundColor = "orange";
	
	row.appendChild(createTableHeaderCell("Ime"));
	row.appendChild(createTableHeaderCell("Godina izdavanja"));
	row.appendChild(createTableHeaderCell("Zanr"));
	row.appendChild(createTableHeaderCell("Bend"));
	row.appendChild(createTableHeaderCell("Broj prodatih primeraka"));
		
	if (jwt_token) {
		row.appendChild(createTableHeaderCell("Akcija"));
		row.appendChild(createTableHeaderCell("Izmena"));
	}

	thead.appendChild(row);
	return thead;
}

function createTableHeaderCell(text) {
	var cell = document.createElement("th");
	var cellText = document.createTextNode(text);
	cell.appendChild(cellText);
	return cell;
}

function createTableCell(text) {
	var cell = document.createElement("td");
	var cellText = document.createTextNode(text);
	cell.appendChild(cellText);
	return cell;
}