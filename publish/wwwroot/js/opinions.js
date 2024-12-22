let pageSize = 10;
let pageNumber = 1;

document.querySelectorAll('.page-size-button').forEach(item => {
    if (parseInt(item.textContent) === pageSize) {
        item.classList.add('active');
    }
    item.addEventListener('click', () => {
        pageSize = parseInt(item.textContent);
        document.querySelectorAll('.page-size-button').forEach(btn => btn.classList.remove('active'));
        item.classList.add('active');
        openPage(1); // reset to first page on page size change
    });
});

const openPage = page => {
    document.querySelector('.opinions').innerHTML = '';
    document.querySelector('.pagination').innerHTML = '';
    axios.post('/Opinie/PobierzOpinie/' + document.querySelector(".service-encoded-name").textContent, {
        "PageSize": pageSize,
        "Page": page,
        "Sorts": "-CreatedAt"
    })
        .then(resp => {
            console.log(resp.data); // Log response for debugging
            appendOpinions(resp.data.items);
            createPagination(resp.data.totalPages, page);
            pageNumber = page;
        }).catch(e => console.log(e.message));
}

const appendOpinions = opinions => {
    const container = document.querySelector('.opinions');
    container.innerHTML = ''; // Clear the container before appending
    for (const opinion of opinions) {
        let opinionsList = `<div class="card mt-5 opinion-card col-md-6">
                <div class="card-body text-center">`;
        for (let i = 0; i < opinion.rating; i++) {
            opinionsList += `<span class="fa-solid fa-star mb-3"></span>`;
        }

        opinionsList += `<p class="card-text">${opinion.content}</p>
                       <small class="text-secondary">- ${opinion.firstName} ${opinion.lastName}</small>
                       <p class="card-text"><strong>Oferta:</strong> ${opinion.offerName}</p>
                       <p class="card-text"><strong>Pracownik:</strong> ${opinion.employeeFullName}</p>
                       </div></div>`;

        container.innerHTML += opinionsList;
    }
}

function createPagination(totalPages, currentPage) {
    const paginationElement = document.querySelector('.pagination');
    paginationElement.innerHTML = '';

    // Przycisk "Poprzedni"
    paginationElement.innerHTML += `
        <li class="page-item ${currentPage === 1 ? 'disabled' : ''}" role="button">
            <a class="page-link" onclick="openPage(${currentPage - 1})">Poprzedni</a>
        </li>
    `;

    // Przyciski z numerami stron
    for (let i = 1; i <= totalPages; i++) {
        if (i === 1 || i === totalPages || (i >= currentPage - 1 && i <= currentPage + 1)) {
            paginationElement.innerHTML += `
                <li class="page-item ${currentPage === i ? 'active' : ''}" role="button">
                    <a class="page-link" onclick="openPage(${i})">${i}</a>
                </li>
            `;
        } else if (i === currentPage - 2 || i === currentPage + 2) {
            paginationElement.innerHTML += `
                <li class="page-item disabled">
                    <a class="page-link">...</a>
                </li>
            `;
        }
    }

    // Przycisk "Następny"
    paginationElement.innerHTML += `
        <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}" role="button">
            <a class="page-link" onclick="openPage(${currentPage + 1})">Następny</a>
        </li>
    `;
}

openPage(pageNumber);
