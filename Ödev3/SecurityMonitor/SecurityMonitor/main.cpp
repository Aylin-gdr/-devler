#include "Monitor.h"
#include <thread>
#include <chrono>
#include <map>


// 1. ÖNEMLÝ: C++'ta fonksiyon tanýmlarý (startMonitoring) main fonksiyonundan ÖNCE 
// ya da main fonksiyonu en altta olacak þekilde yazýlmalýdýr.
void FileMonitor::startMonitoring() {
    // Dosya yollarýný ve son deðiþtirme tarihlerini tutan harita
    std::map<std::string, fs::file_time_type> last_assets;

    try {
        // Önce klasördeki mevcut dosyalarý kaydet
        if (fs::exists(path_to_watch) && fs::is_directory(path_to_watch)) {
            for (auto& p : fs::recursive_directory_iterator(path_to_watch)) {
                last_assets[p.path().string()] = fs::last_write_time(p);
            }
        }
        else {
            std::cout << "Hata: Belirtilen dizin bulunamadi!" << std::endl;
            return;
        }
    }
    catch (const std::exception& e) {
        std::cout << "Erisim Hatasi: " << e.what() << std::endl;
    }

    std::cout << path_to_watch << " izleniyor... Durdurmak icin Ctrl+C\n";

    while (true) {
        std::this_thread::sleep_for(std::chrono::milliseconds(1000)); // 1 saniye bekle

        try {
            for (auto& p : fs::recursive_directory_iterator(path_to_watch)) {
                auto current_path = p.path().string();
                auto current_last_write = fs::last_write_time(p);

                // Yeni dosya mý yoksa deðiþim mi var?
                if (last_assets.find(current_path) == last_assets.end()) {
                    for (auto obs : observers) obs->onNotify("Yeni dosya tespit edildi: " + current_path);
                    last_assets[current_path] = current_last_write;
                }
                else if (last_assets[current_path] != current_last_write) {
                    for (auto obs : observers) obs->onNotify("Dosya degistirildi: " + current_path);
                    last_assets[current_path] = current_last_write;
                }
            }
        }
        catch (...) {
            // Dosya o an baska bir program tarafindan kullaniliyorsa hata vermemesi icin
            continue;
        }
    }
}

int main() {
    // Nesne oluþturma - OOP: Object Creation
    // NOT: Kullanýcý adýndaki boþluk veya Türkçe karakter sorun çýkarmamasý için 
    // masaüstündeki "test" klasörünün yolunu doðru yazdýðýndan emin ol.
    FileMonitor monitor("C:\\Users\\ASUS1\\Desktop\\Ödev\\test");

    SecurityLogger logger;

    // Observer ekleme - OOP: Polymorphism
    monitor.addObserver(&logger);

    // Baþlat
    monitor.startMonitoring();

    return 0;
}