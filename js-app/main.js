const url = "https://localhost:5001/api/beanvariety/";

const button = document.querySelector("#run-button");
button.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            console.log(beanVarieties);
            for (let variety of beanVarieties) {
                document.querySelector(".beanVarieties").innerHTML +=
                    `<li>${variety.name}</li>`
            }

        })
});

function getAllBeanVarieties() {
    return fetch(url).then(resp => resp.json());
}