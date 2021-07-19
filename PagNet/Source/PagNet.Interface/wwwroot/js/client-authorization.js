const _SID = "sid";
const _SESSION_ID_KEY = "TLN-SessionId";

class ClientFormAuthorization {
    static createTokenField(form) {
        var hidden = form.querySelector(`[name=${_SID}]`);
        if (hidden != null) {
            hidden[0].value = sessionStorage.getItem(_SESSION_ID_KEY);;
        }
        else {
            hidden = document.createElement("input");
            hidden.setAttribute("type", "hidden");
            hidden.name = _SID;
            hidden.value = sessionStorage.getItem(_SESSION_ID_KEY);
            form.appendChild(hidden);
        }
    }

    static registerForm(form) {
        if (form === null || form === undefined) return;

        if (form.onsubmit === null || form.onsubmit === undefined) {
            form.onsubmit = (event) => ClientFormAuthorization.createTokenField(event.target);
        }
        else {
            var originalSubmit = form.onsubmit;

            form.onsubmit = (event) => {
                ClientFormAuthorization.createTokenField(event.target)
                originalSubmit(event);
            };
        }
    }

    static registerAllForms() {
        var forms = document.querySelectorAll("form");

        for (var form of forms)
            ClientFormAuthorization.registerForm(form);
    }
}