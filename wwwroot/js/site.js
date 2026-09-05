document.addEventListener("DOMContentLoaded", () => {
    // ==========================================
    // 0. DYNAMIC CLIENT LOCAL GREETING & DATE
    // ==========================================
    const dashboardGreeting = document.getElementById("dashboardGreeting");
    const dashboardDate = document.getElementById("dashboardDate");

    if (dashboardGreeting) {
        const localHour = new Date().getHours();
        let greeting = "Good morning";
        if (localHour >= 12 && localHour < 17) {
            greeting = "Good afternoon";
        } else if (localHour >= 17 && localHour < 22) {
            greeting = "Good evening";
        } else if (localHour >= 22 || localHour < 5) {
            greeting = "Good evening";
        }

        const userName = dashboardGreeting.getAttribute("data-username") || "";
        dashboardGreeting.textContent = `${greeting}, ${userName} 👋`;
    }

    if (dashboardDate) {
        const options = { weekday: "long", year: "numeric", month: "long", day: "numeric" };
        dashboardDate.textContent = new Date().toLocaleDateString(undefined, options);
    }

    // ==========================================
    // 1. FLASH MESSAGE AUTO-DISMISS
    // ==========================================
    setTimeout(() => {
        document.querySelectorAll(".alert.success, .calendar-alert.success").forEach(el => {
            el.style.opacity = "0";
            el.style.transform = "translateY(-6px)";
            el.style.transition = "opacity 0.4s ease, transform 0.4s ease";
            setTimeout(() => el.remove(), 450);
        });
    }, 4000);

    // ==========================================
    // 2. THEME SWITCHER (DARK / LIGHT MODE)
    // ==========================================
    const themeToggleBtn = document.getElementById("themeToggleBtn");
    
    function getCurrentTheme() {
        return document.documentElement.getAttribute("data-theme") || 
               localStorage.getItem("bulbit_theme") || 
               (window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light");
    }

    function applyTheme(theme) {
        document.documentElement.setAttribute("data-theme", theme);
        localStorage.setItem("bulbit_theme", theme);
    }

    if (themeToggleBtn) {
        themeToggleBtn.addEventListener("click", () => {
            const current = getCurrentTheme();
            const nextTheme = current === "dark" ? "light" : "dark";
            applyTheme(nextTheme);
        });
    }

    // ==========================================
    // 3. MOBILE SIDEBAR TOGGLE
    // ==========================================
    const sidebarToggleBtn = document.getElementById("sidebarToggleBtn");
    const appSidebar = document.getElementById("appSidebar");
    const sidebarOverlay = document.getElementById("sidebarOverlay");

    if (sidebarToggleBtn && appSidebar) {
        sidebarToggleBtn.addEventListener("click", () => {
            appSidebar.classList.toggle("open");
            if (sidebarOverlay) sidebarOverlay.classList.toggle("active");
        });

        if (sidebarOverlay) {
            sidebarOverlay.addEventListener("click", () => {
                appSidebar.classList.remove("open");
                sidebarOverlay.classList.remove("active");
            });
        }
    }

    // ==========================================
    // 4. NOTIFICATION SYSTEM
    // ==========================================
    const notificationContainer = document.getElementById("notificationContainer");
    const notificationBtn = document.getElementById("notificationBtn");
    const notificationDropdown = document.getElementById("notificationDropdown");
    const notificationBadge = document.getElementById("notificationBadge");
    const dropdownUnreadCount = document.getElementById("dropdownUnreadCount");
    const notificationList = document.getElementById("notificationList");
    const markAllReadBtn = document.getElementById("markAllReadBtn");

    function getCsrfToken() {
        const tokenInput = document.querySelector("#csrfForm input[name='__RequestVerificationToken']") ||
                           document.querySelector("input[name='__RequestVerificationToken']");
        return tokenInput ? tokenInput.value : "";
    }

    async function fetchNotifications() {
        if (!notificationBtn) return;

        try {
            const res = await fetch("/Notification/GetNotifications", {
                headers: { "X-Requested-With": "XMLHttpRequest" }
            });

            if (!res.ok) return;

            const data = await res.json();
            if (data && data.success) {
                renderNotifications(data.notifications, data.unreadCount);
            }
        } catch (err) {
            console.warn("Could not fetch notifications:", err);
        }
    }

    function renderNotifications(items, unreadCount) {
        // Update badge
        if (unreadCount > 0) {
            notificationBadge.textContent = unreadCount > 99 ? "99+" : unreadCount;
            notificationBadge.style.display = "flex";
            notificationBtn.classList.add("has-unread");
        } else {
            notificationBadge.style.display = "none";
            notificationBtn.classList.remove("has-unread");
        }

        if (dropdownUnreadCount) {
            dropdownUnreadCount.textContent = `${unreadCount} unread`;
        }

        if (!notificationList) return;

        if (!items || items.length === 0) {
            notificationList.innerHTML = `
                <div class="notification-empty">
                    <span class="empty-icon">🔔</span>
                    <p>No notifications yet</p>
                    <small>You're all caught up!</small>
                </div>
            `;
            return;
        }

        let html = "";
        items.forEach(n => {
            const typeIcon = n.type === "Task" ? "✓" : (n.type === "Event" ? "📅" : "🔔");
            const typeClass = (n.type || "general").toLowerCase();
            const unreadClass = !n.isRead ? "unread" : "";

            html += `
                <div class="notification-item ${unreadClass}" data-id="${n.id}" data-url="${escapeHtml(n.url)}">
                    <div class="notification-icon-wrap ${typeClass}">
                        <span>${typeIcon}</span>
                    </div>
                    <div class="notification-content">
                        <div class="notification-title-row">
                            <span class="notification-title">${escapeHtml(n.title)}</span>
                            <span class="notification-time">${escapeHtml(n.timeAgo)}</span>
                        </div>
                        <p class="notification-desc">${escapeHtml(n.message)}</p>
                    </div>
                    ${!n.isRead ? '<span class="unread-dot"></span>' : ''}
                </div>
            `;
        });

        notificationList.innerHTML = html;

        // Attach item click handlers
        notificationList.querySelectorAll(".notification-item").forEach(el => {
            el.addEventListener("click", async (e) => {
                const id = el.getAttribute("data-id");
                const url = el.getAttribute("data-url");

                if (el.classList.contains("unread")) {
                    await markAsRead(id);
                }

                if (url && url !== "#" && url !== "") {
                    let dest = url;
                    if (dest === "/Task") dest = "/Task/Index";
                    else if (dest === "/Calendar") dest = "/Calendar/Index";
                    else if (dest === "/Employee") dest = "/Employee/Index";
                    else if (dest === "/Attendance") dest = "/Attendance/Index";
                    else if (dest === "/Client") dest = "/Client/Index";
                    window.location.href = dest;
                }
            });
        });
    }

    async function markAsRead(id) {
        try {
            const token = getCsrfToken();
            const formData = new URLSearchParams();
            formData.append("id", id);
            formData.append("__RequestVerificationToken", token);

            await fetch("/Notification/MarkAsRead", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: formData.toString()
            });

            fetchNotifications();
        } catch (err) {
            console.error("Failed to mark notification as read:", err);
        }
    }

    async function markAllAsRead() {
        try {
            const token = getCsrfToken();
            const formData = new URLSearchParams();
            formData.append("__RequestVerificationToken", token);

            await fetch("/Notification/MarkAllAsRead", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: formData.toString()
            });

            fetchNotifications();
        } catch (err) {
            console.error("Failed to mark all as read:", err);
        }
    }

    if (markAllReadBtn) {
        markAllReadBtn.addEventListener("click", (e) => {
            e.stopPropagation();
            markAllAsRead();
        });
    }

    if (notificationBtn && notificationDropdown) {
        notificationBtn.addEventListener("click", (e) => {
            e.stopPropagation();
            const isOpen = notificationDropdown.classList.contains("show");
            if (!isOpen) {
                notificationDropdown.classList.add("show");
                notificationBtn.setAttribute("aria-expanded", "true");
                fetchNotifications();
            } else {
                notificationDropdown.classList.remove("show");
                notificationBtn.setAttribute("aria-expanded", "false");
            }
        });

        // Close dropdown when clicking outside
        document.addEventListener("click", (e) => {
            if (notificationContainer && !notificationContainer.contains(e.target)) {
                notificationDropdown.classList.remove("show");
                notificationBtn.setAttribute("aria-expanded", "false");
            }
        });

        // Close dropdown on Escape key
        document.addEventListener("keydown", (e) => {
            if (e.key === "Escape" && notificationDropdown.classList.contains("show")) {
                notificationDropdown.classList.remove("show");
                notificationBtn.setAttribute("aria-expanded", "false");
            }
        });
    }

    // Helper for HTML escaping
    function escapeHtml(str) {
        if (!str) return "";
        return str
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    // Initial fetch and auto-polling every 30 seconds
    if (notificationBtn) {
        fetchNotifications();
        setInterval(fetchNotifications, 30000);
    }
});
