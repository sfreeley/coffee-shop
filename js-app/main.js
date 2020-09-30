const url = "https://localhost:5001/api/beanvariety/";
const postVarietyButton = document.querySelector("#postVarietyButton");
const showNewBeanForm = document.querySelector("#showBeanForm-button");

let nameInput;
let regionInput;
let notesInput;

//displaying all the bean varieties 
const button = document.querySelector("#run-button");
button.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            console.log(beanVarieties);
            for (let variety of beanVarieties) {
                document.querySelector(".beanVarieties").innerHTML +=
                    `
                    <strong>${variety.name}</strong>
                    <li> Region: ${variety.region} </li>
                    <li> Notes: ${variety.notes} </li> 
                    `
            }

        })
    document.querySelector(".beanVarieties").innerHTML = "";
});

//show add new bean form
showNewBeanForm.addEventListener("click", () => {
    document.querySelector(".newBeanForm").classList.toggle("hidden");

})


postVarietyButton.addEventListener("click", () => {
    nameInput = document.querySelector("#name").value;
    regionInput = document.querySelector("#region").value;
    notesInput = document.querySelector("#notes").value;
    postNewBeanVariety()
        .then(newBean => {
            console.log(newBean)
        })
        .then(getAllBeanVarieties());
})


function getAllBeanVarieties() {
    return fetch(url).then(resp => resp.json());
}

function postNewBeanVariety() {
    const newVarietyObject = {
        name: nameInput,
        region: regionInput,
        notes: notesInput
    }
    return fetch(url, {
        method: "POST",
        headers: {
            "Accept": 'application/json',
            "Content-Type": "application/json"
        },
        body: JSON.stringify(newVarietyObject)
    }).then(response => response.json())
}