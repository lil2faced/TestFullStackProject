document.addEventListener('DOMContentLoaded', function() {
    const cart = JSON.parse(localStorage.getItem('cart')) || {};
    const cartItemsContainer = document.getElementById('cart-items');
    const totalPriceElement = document.getElementById('total-price');
    const cartCounter = document.getElementById('cart-counter');
    
    function renderCart() {
        // Очищаем контейнер
        cartItemsContainer.innerHTML = '';
        
        // Если корзина пуста
        if (Object.keys(cart).length === 0) {
            cartItemsContainer.innerHTML = '<p class="empty-cart">Ваша корзина пуста</p>';
            totalPriceElement.textContent = '0';
            cartCounter.textContent = '0';
            return;
        }
        
        let totalPrice = 0;
        
        // Рендерим каждый товар
        Object.values(cart).forEach(item => {
            const itemTotal = item.price * item.quantity;
            totalPrice += itemTotal;
            
            const itemElement = document.createElement('div');
            itemElement.className = 'cart-item';
            itemElement.innerHTML = `
                <div class="cart-item-image">
                    <img src="${item.image}" alt="${item.name}">
                </div>
                <div class="cart-item-info">
                    <h3>${item.name}</h3>
                    <p class="cart-item-price">${item.price} ₽</p>
                </div>
                <div class="cart-item-quantity">
                    <button class="quantity-btn minus">-</button>
                    <span class="quantity">${item.quantity}</span>
                    <button class="quantity-btn plus">+</button>
                </div>
                <div class="cart-item-total">
                    <p>${itemTotal} ₽</p>
                </div>
                <button class="remove-btn">×</button>
            `;
            
            cartItemsContainer.appendChild(itemElement);
            
            // Обработчики для кнопок количества
            const minusBtn = itemElement.querySelector('.minus');
            const plusBtn = itemElement.querySelector('.plus');
            const removeBtn = itemElement.querySelector('.remove-btn');
            const quantityElement = itemElement.querySelector('.quantity');
            
            minusBtn.addEventListener('click', () => {
                if (item.quantity > 1) {
                    item.quantity--;
                    quantityElement.textContent = item.quantity;
                    updateCart();
                }
            });
            
            plusBtn.addEventListener('click', () => {
                item.quantity++;
                quantityElement.textContent = item.quantity;
                updateCart();
            });
            
            removeBtn.addEventListener('click', () => {
                delete cart[item.name];
                updateCart();
            });
        });
        
        totalPriceElement.textContent = totalPrice;
        cartCounter.textContent = Object.values(cart).reduce((sum, item) => sum + item.quantity, 0);
    }
    
    function updateCart() {
        localStorage.setItem('cart', JSON.stringify(cart));
        renderCart();
    }
    
    // Инициализация
    renderCart();
});