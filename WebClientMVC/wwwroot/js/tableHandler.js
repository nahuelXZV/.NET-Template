function showConfirmModal(message) {
    return new Promise((resolve) => {
        const modal = document.getElementById('modal-confirm');
        const content = modal.querySelector('#modalContent');
        const confirmBtn = document.getElementById('btn-modal-confirmar');
        const cancelBtn = document.getElementById('btn-modal-cancelar');

        content.innerHTML = message || '¿Está seguro de continuar?';
        modal.classList.remove('hidden');

        const closeModal = () => {
            modal.classList.add('hidden');
            confirmBtn.removeEventListener('click', confirmHandler);
            cancelBtn.removeEventListener('click', cancelHandler);
        };

        const confirmHandler = () => {
            closeModal();
            resolve(true);
        };

        const cancelHandler = () => {
            closeModal();
            resolve(false);
        };

        confirmBtn.addEventListener('click', confirmHandler);
        cancelBtn.addEventListener('click', cancelHandler);
    });
}

function handleMenuClick(formId, confirm, message) {
    if (confirm) {
        showConfirmModal(message).then(result => {
            if (result) {
                document.getElementById(formId).submit();
            }
        });
    } else {
        document.getElementById(formId).submit();
    }
}

function generateMenuHtml(items, id) {
    return `
    <ul class="py-2 text-sm text-gray-700">
        ${items.map((item, index) => {
        if (item.actionType === 1) {
            const formId = `menuForm_${index}_${id}`;
            return `
								<li>
									<form id="${formId}" action="${item.url}" method="post" class="hidden">
										<input type="hidden" name="id" value="${id}" />
									</form>
									<button onclick="handleMenuClick('${formId}', ${item.confirm ? 'true' : 'false'}, '${item.confirmMessage || ''}'); return false;" class="block px-4 py-2 hover:bg-gray-100">
										${item.iconCss ? `<i class="${item.iconCss} mr-2"></i>` : ''}
										${item.text}
									</button>
								</li>
							`;
        } else {
            return `
								<li>
									<a href="${item.url}?id=${id}" class="block px-4 py-2 hover:bg-gray-100">
										${item.iconCss ? `<i class="${item.iconCss} mr-2"></i>` : ''}
										${item.text}
									</a>
								</li>
							`;
        }
    }).join('')}
    </ul>
    `;
}

function togglePortalDropdown(event, id) {
    event.stopPropagation();
    const button = event.currentTarget;
    const rect = button.getBoundingClientRect();

    let dropdown = document.getElementById('portal-dropdown');
    if (!dropdown) {
        dropdown = document.createElement('div');
        dropdown.id = 'portal-dropdown';
        dropdown.className = "absolute bg-white divide-y divide-gray-100 rounded-lg shadow z-50";
        document.body.appendChild(dropdown);
    }
    dropdown.innerHTML = generateMenuHtml(menuItems, id);

    if (dropdown.style.display === 'block') {
        dropdown.style.display = 'none';
    } else {
        dropdown.style.display = 'block';
        dropdown.style.top = rect.bottom + window.scrollY + 'px';
        dropdown.style.left = rect.left + window.scrollX + 'px';
    }
}

document.addEventListener('click', function (event) {
    const dropdown = document.getElementById('portal-dropdown');
    if (dropdown && !dropdown.contains(event.target) && event.target.id !== 'dropdown-button') {
        dropdown.style.display = 'none';
    }
});

function menuTableHandler(identificador) {
    return `<div class="relative inline-block text-left">
        <button onclick="togglePortalDropdown(event,'${identificador}')" class="inline-flex items-center p-2 text-sm font-medium text-center text-gray-900 bg-white rounded-lg hover:bg-gray-100 focus:outline-none dark:text-white dark:bg-gray-800 dark:hover:bg-gray-700">
            <svg class="w-5 h-5" aria-hidden="true" fill="currentColor" viewBox="0 0 4 15">
                <path d="M3.5 1.5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Zm0 6.041a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Zm0 5.959a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0Z" />
            </svg>
        </button>
    </div>
    `;
}

var optionsTable = {
    search: {
        debounceTimeout: 1000,
        server: {
            url: (prev, keyword) => `${prev}?search=${keyword}`
        }
    },
    pagination: {
        limit: 10,
        resetPageOnUpdate: true,
        server: {
            url: (prev, page, limit) => {
                let inicio = "?";
                if (prev.includes("search")) inicio = "&";
                return `${prev}${inicio}limit=${limit}&offset=${page * limit}`;
            }
        }
    },
    language: {
        'search': {
            'placeholder': 'Buscar...'
        },
        'pagination': {
            'showing': 'Viendo',
            'to': 'a',
            'of': 'de',
            'results': () => 'Resultados',
            'previous': 'Anterior',
            'next': 'Siguiente'
        }
    },
    sort: true,
    resizable: true,
    autoWidth: true,
    className: {
        table: 'text-sm text-left text-gray-500 dark:text-gray-400 ',
        th: 'px-6 py-3 bg-gray-50 ',
        td: 'px-6 py-4 bg-white'
    },
}
