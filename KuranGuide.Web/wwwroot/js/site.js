// ===========================
// Tailwind CDN config
// (Bu dosya, tailwind CDN'den ÖNCE yüklenmeli)
// ===========================

window.tailwind = window.tailwind || {};

tailwind.config = {
    theme: {
        extend: {
            colors: {
                antrasit: '#2C3E50',
                yaprak: '#5A7D7C',
                altin: '#D4AF37',
                gri: '#95A5A6',
                darkbg: '#050816',
                darkcard: '#0B1120'
            },
            fontFamily: {
                arabic: ['Amiri', 'serif'],
                sans: ['Outfit', 'system-ui', 'sans-serif']
            },
            boxShadow: {
                premium: '0 10px 40px -10px rgba(44, 62, 80, 0.12)'
            },
            borderRadius: {
                'xl-card': '1.25rem'
            }
        }
    }
};

// ===========================
// Global yardımcı JS fonksiyonları (ileride kullanmak için)
// ===========================

// Örnek: Tab değiştirme fonksiyonu (istersen HTML tarafında data-attribute ile bağlayabilirsin)
window.switchTab = function (groupName, tabName) {
    const tabs = document.querySelectorAll(`[data-tab-group="${groupName}"] [data-tab]`);
    const contents = document.querySelectorAll(`[data-tab-group="${groupName}"] [data-tab-content]`);

    tabs.forEach(tab => {
        if (tab.dataset.tab === tabName) {
            tab.classList.add('tab-active');
            tab.classList.remove('tab-inactive');
        } else {
            tab.classList.remove('tab-active');
            tab.classList.add('tab-inactive');
        }
    });

    contents.forEach(content => {
        if (content.dataset.tabContent === tabName) {
            content.classList.remove('hidden');
        } else {
            content.classList.add('hidden');
        }
    });
};
