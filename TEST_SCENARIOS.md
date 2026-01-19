# TadesApi Test Senaryoları

## Proje Amacı
E-Fatura entegratörü ile entegrasyon yaparak kontör satışı ve fatura kesme sistemi.

## Mevcut Durum Analizi

### ✅ Var Olanlar:
1. **Customer (Müşteri) Yönetimi**
   - Müşteri ekleme/düzenleme/listeleme
   - VKN/TCKN bilgisi
   - Şirket/Şahıs ayrımı
   - Adres bilgileri

2. **Invoice (Fatura) Yönetimi**
   - Fatura oluşturma endpoint'i
   - Fatura entity'si (GibStatus, GibMessage alanları mevcut)
   - InvoiceItem (Fatura kalemleri)
   - Fatura durumları (Draft, Sent, Approved, Rejected)
   - Senaryo tipleri (Basic, Commercial, Export)

3. **User & Auth Yönetimi**
   - Kullanıcı girişi
   - Yetkilendirme
   - Muhasebeci (Accounter) rolü

### ❌ Eksikler:

1. **GIB Entegratör Entegrasyonu**
   - Entegratör API bağlantısı YOK
   - Fatura gönderme servisi YOK
   - Kontör sorgulama YOK
   - Kontör satın alma YOK

2. **Fatura İşlemleri**
   - Fatura listeleme endpoint'i eksik
   - Fatura detay görüntüleme eksik
   - Fatura güncelleme eksik
   - Fatura silme eksik
   - Fatura PDF oluşturma eksik

3. **Kontör Yönetimi**
   - Kontör bakiye takibi YOK
   - Kontör satın alma YOK
   - Kontör kullanım geçmişi YOK

4. **Raporlama**
   - Fatura raporları YOK
   - Kontör kullanım raporları YOK

## Test Senaryoları

### 1. Müşteri İşlemleri
- [ ] Yeni müşteri ekleme
- [ ] Müşteri listeleme
- [ ] Müşteri güncelleme
- [ ] Müşteri silme

### 2. Fatura İşlemleri
- [ ] Fatura oluşturma (Draft)
- [ ] Fatura listeleme
- [ ] Fatura detay görüntüleme
- [ ] Fatura güncelleme
- [ ] Fatura silme

### 3. GIB Entegrasyonu (YAPILACAK)
- [ ] Entegratör API bağlantısı
- [ ] Kontör bakiye sorgulama
- [ ] Kontör satın alma
- [ ] Fatura GIB'e gönderme
- [ ] GIB durumu sorgulama

## Öncelikli Geliştirmeler

1. **Fatura CRUD İşlemlerini Tamamla**
   - List, GetById, Update, Delete endpoint'leri ekle

2. **GIB Entegratör Servisi Oluştur**
   - Entegratör API client
   - Kontör yönetimi
   - Fatura gönderme

3. **Kontör Yönetimi**
   - Kontör entity'si
   - Kontör işlem geçmişi
   - Bakiye takibi

4. **Raporlama**
   - Fatura raporları
   - Kontör kullanım raporları
