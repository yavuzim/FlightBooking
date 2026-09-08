/* =========================================================
   SkyRoute — script.js
   Index sayfası etkileşimleri (autocomplete YOK, serbest metin girişi)
   ========================================================= */

(function () {
    'use strict';

    /* ---------------------------------------------------------
       1. POPÜLER ROTALAR VERİSİ (yalnızca görsel kart + forma doldurma)
    --------------------------------------------------------- */
    const ROUTES = [
        { fromCity: 'İstanbul', toCity: 'Amsterdam', dur: '3 sa 35 dk', price: '4.799', img: 'https://images.unsplash.com/photo-1534351590666-13e3e96b5017?auto=format&fit=crop&w=800&q=80' },
        { fromCity: 'İstanbul', toCity: 'Milano', dur: '3 sa 05 dk', price: '4.299', img: 'https://images.unsplash.com/photo-1520440229-6469a149ac59?auto=format&fit=crop&w=800&q=80' },
        { fromCity: 'İstanbul', toCity: 'Paris', dur: '3 sa 55 dk', price: '5.150', img: 'https://images.unsplash.com/photo-1502602898657-3e91760cbb34?auto=format&fit=crop&w=800&q=80' },
        { fromCity: 'İstanbul', toCity: 'Roma', dur: '2 sa 55 dk', price: '3.999', img: 'https://images.unsplash.com/photo-1552832230-c0197dd311b5?auto=format&fit=crop&w=800&q=80' },
        { fromCity: 'İstanbul', toCity: 'Berlin', dur: '3 sa 10 dk', price: '4.450', img: 'https://images.unsplash.com/photo-1560969184-10fe8719e047?auto=format&fit=crop&w=800&q=80' },
        { fromCity: 'İstanbul', toCity: 'Londra', dur: '4 sa 05 dk', price: '5.650', img: 'https://images.unsplash.com/photo-1513635269975-59663e0ac1ad?auto=format&fit=crop&w=800&q=80' }
    ];

    /* ---------------------------------------------------------
       Yardımcı: kısa DOM seçici
    --------------------------------------------------------- */
    const $ = (sel, ctx = document) => ctx.querySelector(sel);
    const $$ = (sel, ctx = document) => Array.from(ctx.querySelectorAll(sel));

    /* =========================================================
       2. NAVBAR SCROLL DAVRANIŞI
    ========================================================= */
    const navbar = $('#mainNavbar');
    function handleNavbarScroll() {
        if (window.scrollY > 40) navbar.classList.add('sr-scrolled');
        else navbar.classList.remove('sr-scrolled');
    }
    window.addEventListener('scroll', handleNavbarScroll, { passive: true });
    handleNavbarScroll();

    /* =========================================================
       3. YÖN DEĞİŞTİRME (kalkış <-> varış)
    ========================================================= */
    const fromInput = $('#fromInput');
    const toInput = $('#toInput');
    const swapBtn = $('#swapBtn');

    swapBtn.addEventListener('click', () => {
        const tmp = fromInput.value;
        fromInput.value = toInput.value;
        toInput.value = tmp;
        swapBtn.classList.toggle('sr-rotate');
        clearError(fromInput); clearError(toInput);
    });

    /* =========================================================
       4. SEYAHAT TİPİ — dönüş tarihi göster/gizle
    ========================================================= */
    const returnWrap = $('#returnWrap');
    const returnDate = $('#returnDate');

    function updateTripType() {
        const type = $('input[name="tripType"]:checked').value;
        if (type === 'oneway') {
            returnWrap.style.display = 'none';
            returnDate.value = '';
        } else {
            returnWrap.style.display = '';
        }
    }
    $$('input[name="tripType"]').forEach(r => r.addEventListener('change', updateTripType));
    updateTripType();

    /* =========================================================
       5. TARİH KISITLAMALARI
    ========================================================= */
    const departDate = $('#departDate');
    const today = new Date().toISOString().split('T')[0];
    departDate.min = today;
    returnDate.min = today;

    departDate.addEventListener('change', () => {
        returnDate.min = departDate.value || today;
        if (returnDate.value && returnDate.value < departDate.value) {
            returnDate.value = departDate.value;
        }
        clearError(departDate);
    });

    /* =========================================================
       6. YOLCU SAYACI
    ========================================================= */
    const counts = { adult: 1, child: 0, infant: 0 };
    const MAX_TOTAL = 9;

    const els = {
        adult: $('#adultCount'),
        child: $('#childCount'),
        infant: $('#infantCount')
    };
    const paxNote = $('#paxNote');

    function totalPassengers() { return counts.adult + counts.child + counts.infant; }

    function updatePassengerUI() {
        els.adult.textContent = counts.adult;
        els.child.textContent = counts.child;
        els.infant.textContent = counts.infant;

        $('#passengerSummary').textContent = `${totalPassengers()} Yolcu`;

        $$('.sr-step-btn').forEach(btn => {
            const type = btn.dataset.type;
            const action = btn.dataset.action;
            let disabled = false;

            if (action === 'dec') {
                if (type === 'adult') disabled = counts.adult <= 1;
                else disabled = counts[type] <= 0;
            } else {
                if (totalPassengers() >= MAX_TOTAL) disabled = true;
                if (type === 'infant' && counts.infant >= counts.adult) disabled = true;
            }
            btn.disabled = disabled;
        });

        if (counts.infant >= counts.adult && counts.infant > 0) {
            paxNote.textContent = 'Bebek sayısı yetişkin sayısını geçemez.';
        } else if (totalPassengers() >= MAX_TOTAL) {
            paxNote.textContent = 'En fazla 9 yolcu seçebilirsin.';
        } else {
            paxNote.textContent = '';
        }
    }

    $$('.sr-step-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            const type = btn.dataset.type;
            const action = btn.dataset.action;

            if (action === 'inc') {
                if (totalPassengers() >= MAX_TOTAL) return;
                if (type === 'infant' && counts.infant >= counts.adult) return;
                counts[type]++;
            } else {
                if (type === 'adult' && counts.adult <= 1) return;
                if (counts[type] <= 0) return;
                counts[type]--;
                if (type === 'adult' && counts.infant > counts.adult) counts.infant = counts.adult;
            }
            updatePassengerUI();
        });
    });
    updatePassengerUI();

    /* Kabin sınıfı özeti */
    const cabinClass = $('#cabinClass');
    const cabinLabels = { economy: 'Ekonomi', premium: 'Premium Ekonomi', business: 'Business', first: 'First Class' };
    cabinClass.addEventListener('change', () => {
        $('#cabinSummary').textContent = cabinLabels[cabinClass.value];
    });

    /* Yolcu dropdown "Tamam" butonu — menüyü kapat */
    $('#paxDone').addEventListener('click', () => {
        const toggle = $('#passengerToggle');
        const dd = bootstrap.Dropdown.getInstance(toggle) || new bootstrap.Dropdown(toggle);
        dd.hide();
    });

    /* =========================================================
       7. POPÜLER ROTA KARTLARINI OLUŞTUR
    ========================================================= */
    const routesRow = $('.sr-routes-row');
    routesRow.innerHTML = '';

    ROUTES.forEach(r => {
        const col = document.createElement('div');
        col.className = 'col-12 col-sm-6 col-lg-4';
        col.innerHTML = `
            <div class="sr-route-card">
                <div class="sr-route-media" style="background-image:url('${r.img}')">
                    <span class="sr-route-duration"><i class="bi bi-clock"></i>${r.dur}</span>
                </div>
                <div class="sr-route-body">
                    <div class="sr-route-path">
                        <div class="sr-route-point">
                            <div class="sr-route-city">${r.fromCity}</div>
                        </div>
                        <i class="bi bi-airplane sr-route-arrow"></i>
                        <div class="sr-route-point">
                            <div class="sr-route-city">${r.toCity}</div>
                        </div>
                    </div>
                    <div class="sr-route-price">₺${r.price}'dan başlayan fiyatlarla</div>
                    <button type="button" class="btn btn-sr-outline sr-route-btn">
                        <i class="bi bi-search me-1"></i>Uçuşları Gör
                    </button>
                </div>
            </div>`;
        routesRow.appendChild(col);

        // Karta tıklayınca sadece şehir adlarını forma yaz
        col.querySelector('.sr-route-btn').addEventListener('click', () => {
            fromInput.value = r.fromCity;
            toInput.value = r.toCity;
            clearError(fromInput); clearError(toInput);
            $('#aramaPaneli').scrollIntoView({ behavior: 'smooth', block: 'center' });
            showToast(`${r.fromCity} → ${r.toCity} rotası forma eklendi.`, 'success');
        });
    });

    /* =========================================================
       8. DOĞRULAMA YARDIMCILARI
    ========================================================= */
    function setError(input, message) {
        input.classList.add('is-invalid');
        const wrap = input.closest('.sr-field-wrap');
        if (!wrap) return;
        const errEl = wrap.querySelector('.sr-error');
        if (errEl) errEl.textContent = message;
    }
    function clearError(input) {
        input.classList.remove('is-invalid');
        const wrap = input.closest('.sr-field-wrap');
        if (!wrap) return;
        const errEl = wrap.querySelector('.sr-error');
        if (errEl) errEl.textContent = '';
    }

    // Yazılan metni temizle: baş/son boşluk at, çoklu boşlukları teke indir
    function cleanCity(val) {
        return (val || '').trim().replace(/\s+/g, ' ');
    }

    // Kutuya yazarken hatayı temizle
    [fromInput, toInput].forEach(inp => {
        inp.addEventListener('input', () => clearError(inp));
    });

    /* =========================================================
       9. ARAMA FORMU GÖNDERİMİ
    ========================================================= */
    const form = $('#flightSearchForm');
    const searchBtn = $('#searchBtn');

    form.addEventListener('submit', function (e) {
        e.preventDefault();

        let valid = true;
        const tripType = $('input[name="tripType"]:checked').value;

        const fromCity = cleanCity(fromInput.value);
        const toCity = cleanCity(toInput.value);

        // Kalkış boş mu
        if (!fromCity) { setError(fromInput, 'Kalkış şehri yazın.'); valid = false; }
        else clearError(fromInput);

        // Varış boş mu
        if (!toCity) { setError(toInput, 'Varış şehri yazın.'); valid = false; }
        else clearError(toInput);

        // Aynı şehir olamaz (büyük/küçük harf duyarsız)
        if (fromCity && toCity &&
            fromCity.toLocaleLowerCase('tr') === toCity.toLocaleLowerCase('tr')) {
            setError(toInput, 'Kalkış ve varış aynı olamaz.'); valid = false;
        }

        // Gidiş tarihi
        if (!departDate.value) { setError(departDate, 'Gidiş tarihi seçin.'); valid = false; }
        else clearError(departDate);

        // Dönüş tarihi (yalnızca gidiş-dönüşte zorunlu)
        if (tripType === 'round') {
            if (!returnDate.value) { setError(returnDate, 'Dönüş tarihi seçin.'); valid = false; }
            else if (departDate.value && returnDate.value < departDate.value) {
                setError(returnDate, 'Dönüş, gidişten önce olamaz.'); valid = false;
            } else clearError(returnDate);
        }

        if (!valid) { showToast('Lütfen eksik alanları kontrol et.', 'error'); return; }

        // -------- Parametreleri topla --------
        // Not: IATA çözümlemesi Search sayfasında backend'de yapılıyor.
        // Burada sadece şehir adlarını taşıyoruz.
        const params = new URLSearchParams({
            fromCity: fromCity,
            toCity: toCity,
            depart: departDate.value,
            return: returnDate.value || '',
            adults: counts.adult,
            children: counts.child,
            infants: counts.infant,
            cabin: cabinClass.value,
            currency: $('#currencySelect').value,
            tripType: tripType,
            directOnly: $('#directOnly').checked ? '1' : '0'
        });

        // -------- Loading durumu --------
        searchBtn.disabled = true;
        $('.sr-btn-label', searchBtn).classList.add('d-none');
        $('.sr-btn-loading', searchBtn).classList.remove('d-none');

        // -------- Search sayfasına yönlendir --------
        window.location.href = '/Flight/Search?' + params.toString();
    });

    /* =========================================================
       10. BÜLTEN ABONELİĞİ
    ========================================================= */
    const newsletterForm = $('#newsletterForm');
    const newsletterEmail = $('#newsletterEmail');
    const newsletterError = $('#newsletterError');
    const EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    newsletterForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const val = newsletterEmail.value.trim();
        if (!val) {
            newsletterError.textContent = 'E-posta adresi gerekli.';
            newsletterEmail.classList.add('is-invalid');
        } else if (!EMAIL_RE.test(val)) {
            newsletterError.textContent = 'Geçerli bir e-posta adresi gir.';
            newsletterEmail.classList.add('is-invalid');
        } else {
            newsletterError.textContent = '';
            newsletterEmail.classList.remove('is-invalid');
            newsletterEmail.value = '';
            showToast('Aboneliğiniz başarıyla oluşturuldu.', 'success');
        }
    });
    newsletterEmail.addEventListener('input', () => {
        newsletterEmail.classList.remove('is-invalid');
        newsletterError.textContent = '';
    });

    /* =========================================================
       11. TOAST BİLDİRİMİ
    ========================================================= */
    function showToast(message, type) {
        const toastEl = $('#srToast');
        $('#srToastBody').textContent = message;
        toastEl.classList.remove('sr-toast-success', 'sr-toast-error');
        if (type === 'success') toastEl.classList.add('sr-toast-success');
        if (type === 'error') toastEl.classList.add('sr-toast-error');
        const toast = bootstrap.Toast.getInstance(toastEl) || new bootstrap.Toast(toastEl, { delay: 3500 });
        toast.show();
    }

})();
