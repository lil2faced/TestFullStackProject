document.addEventListener('DOMContentLoaded', function() {
    // Проверяем, первый ли это заход на сайт
    if (!localStorage.getItem('visited')) {
        localStorage.setItem('visited', 'true');
        localStorage.removeItem('cart'); // Очищаем корзину
    }

    let cart = JSON.parse(localStorage.getItem('cart')) || {};
    const cartLink = document.querySelector('.cart a');
    const productButtons = document.querySelectorAll('.product-card .btn');
    const cartCounter = document.getElementById('cart-counter');
    
    function updateCartCounter() {
        const totalItems = Object.values(cart).reduce((sum, item) => sum + item.quantity, 0);
        cartCounter.textContent = totalItems;
        cartLink.innerHTML = `Корзина (<span id="cart-counter">${totalItems}</span>)`;
        localStorage.setItem('cart', JSON.stringify(cart));
    }
    
    // Обработчик для кнопки корзины в шапке
    cartLink.addEventListener('click', function(e) {
        if (Object.keys(cart).length > 0) {
            // Если в корзине есть товары, переходим на страницу корзины
            return true;
        } else {
            // Если корзина пуста, предотвращаем переход
            e.preventDefault();
            alert('Ваша корзина пуста');
        }
    });
    
    productButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            const productCard = this.closest('.product-card');
            const productId = productCard.querySelector('h3').textContent;
            
            if (this.textContent === 'В корзину') {
                // Добавляем товар в корзину
                if (!cart[productId]) {
                    cart[productId] = {
                        name: productId,
                        price: parseInt(productCard.querySelector('.price').textContent),
                        quantity: 1,
                        image: productCard.querySelector('img').src
                    };
                } else {
                    cart[productId].quantity++;
                }
                
                updateCartCounter();
                this.textContent = 'В корзине';
                this.classList.add('in-cart');
            } else {
                // Перенаправляем в корзину
                window.location.href = 'cart.html';
            }
        });
    });

    // Обновляем счетчик при загрузке страницы
    updateCartCounter();
});