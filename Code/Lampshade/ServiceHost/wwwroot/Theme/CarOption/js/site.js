// Car Option Theme - Main JavaScript File

// Mobile menu toggle
function toggleMobileMenu() {
    const mobileMenu = document.getElementById('mobile-menu');
    const menuIcon = document.getElementById('menu-icon');
    
    if (mobileMenu) {
        mobileMenu.classList.toggle('hidden');
        
        if (menuIcon) {
            if (mobileMenu.classList.contains('hidden')) {
                menuIcon.textContent = 'menu';
            } else {
                menuIcon.textContent = 'close';
            }
        }
    }
}

// Mobile search toggle
function toggleMobileSearch() {
    const mobileSearch = document.getElementById('mobile-search');
    if (mobileSearch) {
        mobileSearch.classList.toggle('hidden');
    }
}

// Update cart count display
function updateCartCount(count) {
    const cartCountElements = document.querySelectorAll('.cart-count');
    cartCountElements.forEach(el => {
        if (count > 0) {
            el.textContent = count;
            el.classList.remove('hidden');
        } else {
            el.classList.add('hidden');
        }
    });
}

// Add to cart functionality
function addToCart(productId, productName) {
    // This will be integrated with the existing cart system
    console.log('Adding to cart:', productId, productName);
    
    // Show notification
    showNotification('محصول به سبد خرید اضافه شد', 'success');
}

// Show notification toast
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `fixed bottom-4 left-4 z-50 px-6 py-3 rounded-lg text-white font-medium shadow-lg transform transition-all duration-300 translate-y-full opacity-0`;
    
    switch (type) {
        case 'success':
            notification.classList.add('bg-green-600');
            break;
        case 'error':
            notification.classList.add('bg-red-600');
            break;
        default:
            notification.classList.add('bg-primary');
    }
    
    notification.textContent = message;
    document.body.appendChild(notification);
    
    // Animate in
    setTimeout(() => {
        notification.classList.remove('translate-y-full', 'opacity-0');
    }, 10);
    
    // Remove after 3 seconds
    setTimeout(() => {
        notification.classList.add('translate-y-full', 'opacity-0');
        setTimeout(() => {
            notification.remove();
        }, 300);
    }, 3000);
}

// Scroll to top functionality
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Show/hide scroll to top button
function handleScrollToTopButton() {
    const scrollTopBtn = document.getElementById('scroll-top-btn');
    if (scrollTopBtn) {
        if (window.scrollY > 300) {
            scrollTopBtn.classList.remove('opacity-0', 'pointer-events-none');
        } else {
            scrollTopBtn.classList.add('opacity-0', 'pointer-events-none');
        }
    }
}

// Initialize on DOM ready
document.addEventListener('DOMContentLoaded', function() {
    // Handle scroll to top button visibility
    window.addEventListener('scroll', handleScrollToTopButton);
    
    // Initialize any existing cart data
    if (typeof updateCart === 'function') {
        updateCart();
    }
});

// Price formatting helper (Persian)
function formatPrice(price) {
    return new Intl.NumberFormat('fa-IR').format(price);
}

// Image lazy loading fallback
function handleImageError(img) {
    img.src = '/Theme/CarOption/images/placeholder.png';
    img.alt = 'تصویر در دسترس نیست';
}
