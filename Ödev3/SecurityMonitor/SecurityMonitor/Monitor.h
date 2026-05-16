#pragma once
#include <iostream>
#include <string>
#include <filesystem> // Mutlaka olmalý
#include <vector>

namespace fs = std::filesystem;
// 1. Arayüz
class IObserver {
public:
    virtual void onNotify(const std::string& message) = 0;
    virtual ~IObserver() {}
};

// 2. Logger (Bunu Monitor'ün üstüne taþýdýk)
class SecurityLogger : public IObserver {
public:
    void onNotify(const std::string& message) override {
        std::cout << "[GUVENLIK UYARISI]: " << message << std::endl;
    }
};

// 3. Ýzleyici Sýnýfý
class FileMonitor {
private:
    std::string path_to_watch;
    std::vector<IObserver*> observers;

public:
    FileMonitor(std::string path) : path_to_watch(path) {}

    void addObserver(IObserver* observer) {
        observers.push_back(observer);
    }

    void startMonitoring();
};