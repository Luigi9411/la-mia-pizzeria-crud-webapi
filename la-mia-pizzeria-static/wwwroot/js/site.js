const loadPizzas = filter => getPizzas(filter).then(renderPizzas);

const getPizzas = name => axios
    .get('/Api/PizzasApi', name ? { params: { name } } : {})
    .then(res => res.data);

const renderPizzas = pizzas => {
    const pizzaCards = document.getElementById('pizza-cards');
    pizzaCards.innerHTML = pizzas.map(pizzaComponent).join('');
}

const pizzaComponent = pizza => `
    
    <li class="me-4">
    <div class="card" style="width: 18rem;">
        <img src="<${pizza.Image}" class="card-img-top" alt="Americana" style="height: 12rem;">
        <div class="card-body">
            <h4>
            <a class="card-title ms-lg-3" href="@Url.Action("Detail", "Pizza", new { Id = pizza.Id })">${pizza.name}</a>
            </h4>
            <p class="card-text ms-lg-3">${pizza.description}</p>
            <p class ="card-text ms-lg-3">${pizza.category.name}</p>
            <h6 class="ms-lg-3">${pizza.price}</h6>
            <div class="bt">
                <button onClick="deletePizza(${pizza.id})" type="submit" class="btn btn-secondary mt-3">
                      Delete
              </button>
            </div>
        </div>
    </div>
</li>
    `;

function deletePizza(id) {
    axios.delete(`/Api/PizzasApi/${id}`)
        .then(function (response) {
            console.log(response)
        }).catch(function (error) {
            console.log(error)
        });
    location.reload()
}


const loadCategories = () => getCategories().then(renderCategories);

const getCategories = () => axios
    .get("/Api/Category")
    .then(res => res.data);

const renderCategories = categories => {
    const selectCategory = document.querySelector("#categoryId");
    selectCategory.innerHTML += categories.map(categoryOptionComponent).join('');
};

const categoryOptionComponent = category => `<option value=${category.id}>${category.name}</option>`;

// </Categories>

// <Ingredienti>

const loadIngredients = () => getIngredients().then(renderIngredients);

const getIngredients = () => axios
    .get("/Api/Ingredient")
    .then(res => res.data);

const renderIngredients = ingredients => {
    const IngredientiSelection = document.querySelector("#ingredients");
    IngredientsSelection.innerHTML = ingredients.map(ingredientOptionComponent).join('');
}

const ingredientOptionComponent = ingredient => `
	<div class="flex gap">
		<input id="${ingredient.id}" type="checkbox" />
		<label for="${ingredient.id}">${ingredient.name}</label>
	</div>`;

// </Ingredienti>

// <CreatePizza>

const pizzaPizza = pizza => axios
    .post("/Api/PizzasApi", pizza)
    .then(() => location.href = "/Pizza/ApiIndex")
    .catch(err => renderErrors(err.response.data.errors));

const initCreateForm = () => {
    const form = document.querySelector("#pizza-create-form");

    form.addEventListener("submit", e => {
        e.preventDefault();

        const pizza = getPizzaFromForm(form);
        pizzaPizza(pizza);
    });
};

const getPizzaFromForm = form => {
    const name = form.querySelector("#name").value;
    const description = form.querySelector("#description").value;
    const image = form.querySelector("#image").value;
    const categoryId = form.querySelector("#categoryId").value;
    const price = form.querySelector("#price").value;

    return {
        name,
        description,
        image,
        categoryId,
        price
    };
};

const renderErrors = errors => {
    const nameErrors = document.querySelector("#name-errors");
    const descriptionErrors = document.querySelector("#description-errors");
    const imageErrors = document.querySelector('#image-error');
    const priceErrors = document.querySelector('#price-error');
    const categoryIdErrors = document.querySelector("#categoryId-errors");

    nameErrors.innerText = errors.Name?.join('\n') ?? '';
    descriptionErrors.innerText = errors.Description?.join('\n') ?? '';
    imageErrors.innerText = errors.image?.join('\n') ?? '';
    priceErrors.innerText = errors.Price?.join('\n') ?? '';
    categoryIdErrors.innerText = errors.CategoryId?.join('\n') ?? '';
};

