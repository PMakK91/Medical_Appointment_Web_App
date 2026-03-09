document.addEventListener("DOMContentLoaded", () => {
    // -------- Tabs: show/hide forms --------
    $(function () {
        const $tabs = $(".register-tabs .register-tab");
        const $formPatient = $("#register-form-patient");
        const $formDoctor = $("#register-form-doctor");
        const $tabsWrap = $(".register-tabs");

        function setActiveTab(idx) {
            $tabs.each(function (i) {
                const active = i === idx;

                $(this)
                    .toggleClass("active", active)
                    .attr("aria-selected", active ? "true" : "false");
            });

            if (idx === 0) {
                $formPatient.removeClass("d-none");
                $formDoctor.addClass("d-none");
                $tabsWrap.removeClass("doctor-active");
            } else {
                $formDoctor.removeClass("d-none");
                $formPatient.addClass("d-none");
                $tabsWrap.addClass("doctor-active");
            }
        }

        $tabs.on("click", function () {
            setActiveTab($(this).index());
        });

        setActiveTab(0); // default tab
    });

    // -------- Password eye toggle (works for all) --------

    $(document).on("click", ".password-toggle", function () {
        const $btn = $(this);
        const $wrapper = $btn.closest(".password-wrapper");
        if (!$wrapper.length) return;

        const $input = $wrapper.find('input[type="password"], input[type="text"]').first();
        if (!$input.length) return;

        const $openIcon = $btn.find(".eye-open");
        const $closedIcon = $btn.find(".eye-closed");

        const isHidden = $input.attr("type") === "password";

        $input.attr("type", isHidden ? "text" : "password");

        $openIcon.toggleClass("d-none", isHidden);
        $closedIcon.toggleClass("d-none", !isHidden);

        $input.trigger("focus");
    });


    /***** FILL FORMS PATIENT AND DOCTOR SIMUTANEUSLY *******/

    (function () {
        const patientForm = document.getElementById('register-form-patient');
        const doctorForm = document.getElementById('register-form-doctor');
        if (!patientForm || !doctorForm) return;

        const fieldsToSync = [
            'FirstName', 'LastName', 'Phone', 'Amka', 'Email', 'Password',
            'Gender', 'BirthDate'
        ];

        const isVisible = (el) => !!el && !el.classList.contains('d-none');

        function getField(form, name) {
            return form.querySelector(`[name="${name}"]`);
        }

        function getCheckedRadio(form, name) {
            return form.querySelector(`input[type="radio"][name="${name}"]:checked`);
        }

        function getRadios(form, name) {
            return form.querySelectorAll(`input[type="radio"][name="${name}"]`);
        }

        function syncValue(fromForm, toForm, name) {
            // RADIO GROUP
            const fromAnyRadio = fromForm.querySelector(`input[type="radio"][name="${name}"]`);
            const toAnyRadio = toForm.querySelector(`input[type="radio"][name="${name}"]`);
            if (fromAnyRadio && toAnyRadio) {
                const fromChecked = getCheckedRadio(fromForm, name);
                if (!fromChecked) return; // μην γράφεις τίποτα αν δεν έχει επιλεγεί

                const target = toForm.querySelector(
                    `input[type="radio"][name="${name}"][value="${fromChecked.value}"]`
                );
                if (target && !target.checked) target.checked = true;
                return;
            }

            // NORMAL INPUTS (text/tel/email/password/date/hidden etc.)
            const from = getField(fromForm, name);
            const to = getField(toForm, name);
            if (!from || !to) return;

            // ΜΗΝ αντιγράφεις κενές τιμές (για να μην "σβήνει" το άλλο form)
            if (from.value == null || from.value === '') return;

            if (to.value !== from.value) {
                to.value = from.value;
            }
        }

        function wireSync(fromForm, toForm) {
            fieldsToSync.forEach(name => {
                // radio group
                const radios = getRadios(fromForm, name);
                if (radios && radios.length) {
                    radios.forEach(r => {
                        r.addEventListener('change', () => syncValue(fromForm, toForm, name));
                    });
                    return;
                }

                // normal field
                const from = getField(fromForm, name);
                if (!from) return;

                from.addEventListener('input', () => syncValue(fromForm, toForm, name));
                from.addEventListener('change', () => syncValue(fromForm, toForm, name));
            });
        }

        wireSync(patientForm, doctorForm);
        wireSync(doctorForm, patientForm);

        // ✅ Initial sync: από το ορατό form προς το κρυφό
        const source = isVisible(patientForm) ? patientForm : doctorForm;
        const target = source === patientForm ? doctorForm : patientForm;
        fieldsToSync.forEach(name => syncValue(source, target, name));
    })();
    

    // ************ Specialization dropdown with search inside (doctor) **********/
    $(function () {
        const $btn = $("#specializationDropdownBtn");
        const $selectedText = $("#specializationSelectedText");
        const $hidden = $("#Specialization");
        const $menu = $("#specializationMenu");
        const $search = $("#specializationSearch");
        const $list = $("#specializationList");
        const $noResults = $("#specializationNoResults");
        const $errorEl = $("#specializationError");
        const $doctorForm = $("#register-form-doctor");

        if ($btn.length && $selectedText.length && $hidden.length && $menu.length && $search.length && $list.length) {
            const $items = $list.find(".dropdown-item"); 
            const dd = bootstrap.Dropdown.getOrCreateInstance($btn[0], { autoClose: "outside" });

            const normalize = (s) =>
                (s || "")
                    .toLowerCase()
                    .normalize("NFD")
                    .replace(/[\u0300-\u036f]/g, "");

            const showError = (msg) => {
                if (!$errorEl.length) return;
                $errorEl.text(msg).removeClass("d-none");
            };

            const hideError = () => {
                if (!$errorEl.length) return;
                $errorEl.text("").addClass("d-none");
            };

            const filter = () => {
                const q = normalize($.trim($search.val()));
                let any = false;

                $items.each(function () {
                    const $it = $(this);
                    const t = normalize($it.text());
                    const show = q === "" || t.includes(q);

                    $it.closest("li").toggle(show);
                    if (show) any = true;
                });

                if ($noResults.length) $noResults.toggleClass("d-none", any);
            };

            // When opening, reset & focus search
            $btn.on("click", function () {
                setTimeout(function () {
                    $search.val("");
                    filter();
                    $search.trigger("focus");
                }, 0);
            });

            $search.on("input", filter);

            // Item click (use delegation in case items change later)
            $list.on("click", ".dropdown-item", function () {
                const $it = $(this);

                const label = $.trim($it.text());
                const value = $it.data("value") || label;

                $selectedText.text(label).removeClass("placeholder");
                $hidden.val(value);

                $items.removeClass("active");
                $it.addClass("active");

                hideError();
                dd.hide();
            });

            // Prevent clicks inside menu from closing dropdown
            $menu.on("click", function (e) {
                e.stopPropagation();
            });

            // Validate on submit
            if ($doctorForm.length) {
                $doctorForm.on("submit", function (e) {
                    if (!$.trim($hidden.val())) {
                        e.preventDefault();
                        showError("Παρακαλώ επιλέξτε ειδικότητα από τη λίστα.");
                        dd.show();
                        setTimeout(() => $search.trigger("focus"), 0);
                    }
                });
            }
        }
    });
});
/****** WEEK SCROLLER *******/
function showOffice(officeId) {
    document.querySelectorAll(".office-panel").forEach(p => p.style.display = "none");

    const panel = document.getElementById("office-" + officeId);
    if (panel) panel.style.display = "block";

    document.querySelectorAll(".office-tab").forEach(b => {
        b.classList.toggle("active", b.dataset.officeId == officeId);
    });
}

function loadOffice(practiceId, weekStart = null) {
    const officePanel = $("#office-" + practiceId);
    const scheduleWrap = officePanel.find(".schedule-wrap");

    let url = `/Practice/AvailableTimeSlots?Id=${practiceId}`;
    if (weekStart) {
        url += `&weekStart=${weekStart}`;
    }

    scheduleWrap.load(url, function () {
        officePanel.data("loaded", true);
    });
}

$(document).ready(function () {
    const firstVisibleOffice = $(".office-panel:visible").first();

    if (firstVisibleOffice.length) {
        const firstId = firstVisibleOffice.attr("id").replace("office-", "");
        loadOffice(firstId);
    }

    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (mutation.attributeName === "style") {
                const element = $(mutation.target);

                if (element.is(":visible")) {
                    const practiceId = element.attr("id").replace("office-", "");
                    loadOffice(practiceId);
                }
            }
        });
    });

    $(".office-panel").each(function () {
        observer.observe(this, { attributes: true });
    });
});

function scrollDays(direction, practiceId) {
    const container = document.querySelector(`#daysScroller-${practiceId}`);
    const officePanel = document.querySelector(`#office-${practiceId}`);
    const scheduleWrap = officePanel ? officePanel.querySelector(".schedule-wrap") : null;

    if (!container || !scheduleWrap) return;

    const startDateStr = container.dataset.startdate;
    const startDate = new Date(startDateStr);
    startDate.setDate(startDate.getDate() + direction);

    const year = startDate.getFullYear();
    const month = String(startDate.getMonth() + 1).padStart(2, "0");
    const day = String(startDate.getDate()).padStart(2, "0");
    const newDate = `${year}-${month}-${day}`;

    scheduleWrap.style.transition = "transform 0.2s ease, opacity 0.2s ease";
    scheduleWrap.style.transform = `translateX(${direction * -10}px)`;
    scheduleWrap.style.opacity = "0";

    setTimeout(() => {
        fetch(`/Practice/AvailableTimeSlots?Id=${practiceId}&weekStart=${newDate}`)
            .then(response => response.text())
            .then(html => {
                scheduleWrap.innerHTML = html;

                scheduleWrap.style.transition = "none";
                scheduleWrap.style.transform = `translateX(${direction * 10}px)`;
                scheduleWrap.style.opacity = "0";

                void scheduleWrap.offsetWidth;

                scheduleWrap.style.transition = "transform 0.2s ease, opacity 0.2s ease";
                scheduleWrap.style.transform = "translateX(0)";
                scheduleWrap.style.opacity = "1";
            })
            .catch(err => console.error("Schedule load error:", err));
    }, 200);
}
/********************** CALENDAR ************************/

const officeCalendarState = {};

/*FETCH PRACTICE*/
function fetchPracticeInfo(practiceId) {
    return fetch(`/Practice/Get/${practiceId}`).then(async (response) => {
        if (!response.ok) {
            const text = await response.text().catch(() => "");
            throw new Error(`HTTP ${response.status} ${response.statusText} ${text}`);
        }
        return response.json();
    });
}

/* Schedule helpers */
function toHHmm(t) {
    return typeof t === "string" && t.length >= 5 ? t.slice(0, 5) : null; // "HH:mm:ss" -> "HH:mm"
}

function getField(obj, pascalName) {
    if (!obj) return undefined;
    const camel = pascalName.charAt(0).toLowerCase() + pascalName.slice(1);
    return obj[pascalName] ?? obj[camel];
}

function getOfficeScheduleConfig(practice, dateStr) {
    if (!practice) return { duration: 30, workingIntervals: [] };

    const duration = 30; // or read from practice if needed

    const d = new Date(`${dateStr}T00:00:00`);
    const dow = (d.getDay() + 6) % 7; // 0=Mon ... 6=Sun

    const dayName = [
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday",
        "Sunday",
    ][dow];

    const startKey = `${dayName}Start`;
    const endKey = `${dayName}End`;

    const start = toHHmm(getField(practice, startKey));
    const end = toHHmm(getField(practice, endKey));

    if (!start || !end) return { duration, workingIntervals: [] };

    return {
        duration,
        workingIntervals: [{ start, end }],
    };
}

function isOfficeClosed(practice, dateStr) {
    return getOfficeScheduleConfig(practice, dateStr).workingIntervals.length === 0;
}

/** ---------------------------
 *  INIT
 *  --------------------------- */
document.addEventListener("DOMContentLoaded", async () => {
    const containers = Array.from(document.querySelectorAll(".calendarContainer"));

    await Promise.all(
        containers.map(async (calendarContainer) => {
            const officeId = calendarContainer.dataset.officeId; // Practice.Id GUID

            officeCalendarState[officeId] = {
                currentDate: new Date(),
                view: "month",
                practice: null,
            };

            try {
                officeCalendarState[officeId].practice = await fetchPracticeInfo(officeId);

                // Debug (uncomment to verify values)
                // console.log("Loaded practice:", officeId, officeCalendarState[officeId].practice);
                // console.log("Mon:", getField(officeCalendarState[officeId].practice, "MondayStart"), getField(officeCalendarState[officeId].practice, "MondayEnd"));
                // console.log("Tue:", getField(officeCalendarState[officeId].practice, "TuesdayStart"), getField(officeCalendarState[officeId].practice, "TuesdayEnd"));
            } catch (e) {
                console.error("Failed to load practice info for:", officeId, e);
            }

            await renderCurrentView(calendarContainer);
        })
    );
});

async function renderCurrentView(container) {
    const officeId = container.dataset.officeId;
    const state = officeCalendarState[officeId];

    if (state.view === "month") {
        await generateMonthView(container, state.currentDate);
    } else {
        generateDayView(container, state.currentDate);
    }
}

/**** DAY VIEW ****/
function generateDayView(calendarContainer, dateObj) {
    const officeId = calendarContainer.dataset.officeId;
    const practice = officeCalendarState[officeId]?.practice;

    const dayNames = ["Κυριακή", "Δευτέρα", "Τρίτη", "Τετάρτη", "Πέμπτη", "Παρασκευή", "Σάββατο"];
    const monthNames = [
        "Ιανουαρίου",
        "Φεβρουαρίου",
        "Μαρτίου",
        "Απριλίου",
        "Μαϊου",
        "Ιουνίου",
        "Ιουλίου",
        "Αυγούστου",
        "Σεπτεμβρίου",
        "Οκτωβρίου",
        "Νοεμβρίου",
        "Δεκεμβρίου",
    ];

    const oldHeading = calendarContainer.querySelector(".month");
    if (oldHeading) oldHeading.remove();

    const headingElement = document.createElement("div");
    headingElement.classList.add("month");
    headingElement.innerText = `${dayNames[dateObj.getDay()]} ${dateObj.getDate()} ${monthNames[dateObj.getMonth()]
        } ${dateObj.getFullYear()}`;

    const navButtons = calendarContainer.querySelector(".nav-buttons-container");
    navButtons.innerHTML = "";

    const prevButton = document.createElement("button");
    prevButton.innerHTML = "‹";

    const nextButton = document.createElement("button");
    nextButton.innerHTML = "›";

    prevButton.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        const nd = new Date(st.currentDate);
        nd.setDate(nd.getDate() - 1);
        st.currentDate = nd;
        void renderCurrentView(calendarContainer);
    });

    nextButton.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        const nd = new Date(st.currentDate);
        nd.setDate(nd.getDate() + 1);
        st.currentDate = nd;
        void renderCurrentView(calendarContainer);
    });

    navButtons.appendChild(prevButton);
    navButtons.appendChild(headingElement);
    navButtons.appendChild(nextButton);

    const inner = calendarContainer.querySelector(".calendarInnerContainer");
    inner.innerHTML = "";

    const backMonthBtn = document.createElement("button");
    backMonthBtn.className = "fs-14 bg-transparent fc-blue mb-3 border-0";
    backMonthBtn.textContent = "‹ Προβολή όλου του μήνα";
    backMonthBtn.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        st.view = "month";
        void renderCurrentView(calendarContainer);
    });

    const dayDiv = document.createElement("div");
    dayDiv.classList.add("dayView");

    const dateStr = formatLocalYYYYMMDD(dateObj);
    dayDiv.dataset.date = dateStr;
    dayDiv.dataset.officeId = officeId;

    inner.appendChild(backMonthBtn);
    inner.appendChild(dayDiv);

    fetchAppointmentsForDate(officeId, practice, dateStr, dayDiv);
}

/** ---------------------------
 *  DAY RENDERER
 *  --------------------------- */
function displayAppointmentsForDate(appointments, dayDiv, duration, workingIntervals) {
    if (!dayDiv) return;
    if (!Array.isArray(appointments)) appointments = [];

    dayDiv.innerHTML = "";

    const slotsContainer = document.createElement("div");
    slotsContainer.classList.add("slots-container");

    if (!workingIntervals || workingIntervals.length === 0) {
        const msg = document.createElement("div");
        msg.classList.add("slot", "not-available");
        msg.textContent = "Κλειστό";
        slotsContainer.appendChild(msg);
        dayDiv.appendChild(slotsContainer);
        return;
    }

    const apptByTime = new Map();
    appointments.forEach((a) => {
        if (a && a.time) apptByTime.set(a.time.split("T")[1].slice(0, 5), a);
    });

    const interval = workingIntervals[0];
    const startMinutes = timeToMinutesHHMM(interval.start);
    const endMinutes = timeToMinutesHHMM(interval.end);

    for (let mins = startMinutes; mins < endMinutes; mins += duration) {
        const timeStr = minutesToHHMM(mins);

        const slotDiv = document.createElement("div");
        slotDiv.classList.add("slot");
        slotDiv.dataset.time = timeStr;

        const timeLabel = document.createElement("span");
        timeLabel.classList.add("slot-time");
        timeLabel.textContent = timeStr;
        slotDiv.appendChild(timeLabel);

        const appt = apptByTime.get(timeStr);

        if (appt) {
            slotDiv.classList.add("not-available");

            const name = document.createElement("span");
            name.classList.add("slot-name");
            name.textContent =
                appt.patientFirstName && appt.patientLastName
                    ? `${appt.patientFirstName} ${appt.patientLastName}`
                    : "Appointment";
            slotDiv.appendChild(name);

            slotDiv.style.cursor = "pointer";
            slotDiv.addEventListener("click", (e) => {
                e.stopPropagation();
                window.location.href = `/appointments/details/${appt.id}`;
            });
        } else {
            slotDiv.classList.add("available");

            const addButton = document.createElement("button");
            addButton.className = "addAppointmentButton";
            addButton.style.width = "30px";
            addButton.style.height = "30px";
            addButton.textContent = "+";

            addButton.addEventListener("click", (e) => {
                e.stopPropagation();
                const date = dayDiv.dataset.date;
                const officeId = dayDiv.dataset.officeId;
                window.location.href = `/appointments/create?officeId=${officeId}&date=${date}&time=${timeStr}`;
            });

            slotDiv.appendChild(addButton);
        }

        slotsContainer.appendChild(slotDiv);
    }

    dayDiv.appendChild(slotsContainer);
}

/** ---------------------------
 *  MONTH VIEW
 *  --------------------------- */
async function generateMonthView(calendarContainer, dateObj) {
    const officeId = calendarContainer.dataset.officeId;
    const practice = officeCalendarState[officeId]?.practice;

    const inner = calendarContainer.querySelector(".calendarInnerContainer");
    const navButtons = calendarContainer.querySelector(".nav-buttons-container");
    if (!inner || !navButtons) return;

    const oldHeading = calendarContainer.querySelector(".month");
    if (oldHeading) oldHeading.remove();

    inner.innerHTML = "";

    const monthNames = [
        "Ιανουάριος",
        "Φεβρουάριος",
        "Μάρτιος",
        "Απρίλιος",
        "Μάιος",
        "Ιούνιος",
        "Ιούλιος",
        "Αύγουστος",
        "Σεπτέμβριος",
        "Οκτώβριος",
        "Νοέμβριος",
        "Δεκέμβριος",
    ];
    const weekDaysShort = ["Δευ", "Τρι", "Τετ", "Πεμ", "Παρ", "Σαβ", "Κυρ"];

    const year = dateObj.getFullYear();
    const month = dateObj.getMonth();

    const headingElement = document.createElement("div");
    headingElement.classList.add("month");
    headingElement.innerText = `${monthNames[month]} ${year}`;

    navButtons.innerHTML = "";

    const prevButton = document.createElement("button");
    prevButton.innerHTML = "‹";

    const nextButton = document.createElement("button");
    nextButton.innerHTML = "›";

    navButtons.appendChild(prevButton);
    navButtons.appendChild(headingElement);
    navButtons.appendChild(nextButton);

    prevButton.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        const d = new Date(st.currentDate);
        d.setMonth(d.getMonth() - 1);
        st.currentDate = d;
        void renderCurrentView(calendarContainer);
    });

    nextButton.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        const d = new Date(st.currentDate);
        d.setMonth(d.getMonth() + 1);
        st.currentDate = d;
        void renderCurrentView(calendarContainer);
    });

    const mode = getLayoutMode();

    const grid = document.createElement("div");
    grid.classList.add(mode === "mobile" ? "month-list" : "month-grid");
    if (mode === "tablet") grid.classList.add("month-grid-tablet");

    if (mode !== "mobile") {
        weekDaysShort.forEach((w) => {
            const h = document.createElement("div");
            h.classList.add("month-header");
            h.textContent = w;
            grid.appendChild(h);
        });
    }

    let firstDay = new Date(year, month, 1).getDay(); // 0=Sun
    firstDay = (firstDay + 6) % 7; // Monday=0

    const daysInMonth = new Date(year, month + 1, 0).getDate();

    if (mode !== "mobile") {
        for (let i = 0; i < firstDay; i++) {
            const empty = document.createElement("div");
            empty.classList.add("month-cell", "empty");
            grid.appendChild(empty);
        }
    }

    const monthFirst = `${year}-${String(month + 1).padStart(2, "0")}-01`;
    const appointments = await fetchAppointmentsForMonth(officeId, monthFirst);

    for (let day = 1; day <= daysInMonth; day++) {
        const cell = document.createElement("div");
        cell.classList.add("month-cell");

        const dateStr = `${year}-${String(month + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;
        const closed = isOfficeClosed(practice, dateStr);

        if (closed) cell.classList.add("month-cell-closed");

        const dayNum = document.createElement("div");
        dayNum.classList.add("month-day-number");
        dayNum.textContent = day;
        cell.appendChild(dayNum);

        if (!closed) {
            const { duration, workingIntervals } = getOfficeScheduleConfig(practice, dateStr);

            const appts = getAppointmentsByDay(appointments, year, month + 1, day);
            const booked = appts.length;

            const totalSlots = calculateTotalSlots(duration, workingIntervals);
            const free = Math.max(0, totalSlots - booked);

            const summary = document.createElement("div");
            summary.classList.add("month-summary");

            const layoutMode = getLayoutMode();

            if (booked > 0) {
                const bookedEl = document.createElement("p");
                bookedEl.classList.add("month-booked");
                bookedEl.textContent = layoutMode === "tablet" ? `${booked}` : `Κλεισμένα: ${booked}`;
                summary.appendChild(bookedEl);
            }

            if (free > 0) {
                const freeEl = document.createElement("p");
                freeEl.classList.add("month-available");
                freeEl.textContent = layoutMode === "tablet" ? `${free}` : `Διαθέσιμα: ${free}`;
                summary.appendChild(freeEl);
            }

            if (summary.children.length > 0) cell.appendChild(summary);
        }

        const today = new Date();
        if (year === today.getFullYear() && month === today.getMonth() && day === today.getDate()) {
            cell.classList.add("today");
        }

        if (!closed) {
            cell.style.cursor = "pointer";
            cell.addEventListener("click", () => {
                const st = officeCalendarState[officeId];
                st.currentDate = new Date(`${dateStr}T00:00:00`);
                st.view = "day";
                void renderCurrentView(calendarContainer);
            });
        } else {
            cell.style.cursor = "not-allowed";
        }

        grid.appendChild(cell);
    }

    inner.appendChild(grid);
}

/** ---------------------------
 *  APPOINTMENTS
 *  --------------------------- */
function fetchAppointmentsForDate(officeId, practice, dateStr, dayDiv) {
    const { duration, workingIntervals } = getOfficeScheduleConfig(practice, dateStr);

    fetch(`/Appointment/GetAppointmentsByDayAndPracticeId?date=${dateStr}&practiceId=${officeId}`)
        .then((response) => response.json())
        .then((appointments) => {
            displayAppointmentsForDate(appointments, dayDiv, duration, workingIntervals);
        })
        .catch((e) => {
            console.error("Failed to fetch appointments for day:", officeId, dateStr, e);
            displayAppointmentsForDate([], dayDiv, duration, workingIntervals);
        });
}

async function fetchAppointmentsForMonth(officeId, dateStr) {
    return fetch(`/Appointment/GetAppointmentsByMonthAndPracticeId?date=${dateStr}&practiceId=${officeId}`)
        .then((response) => response.json())
        .then((appointments) => appointments)
        .catch((e) => {
            console.error("Failed to fetch appointments for month:", officeId, dateStr, e);
            return [];
        });
}

function getAppointmentsByDay(appointments, year, month, day) {
    return (appointments || []).filter((app) => {
        const [y, m, d] = app.time.slice(0, 10).split("-");
        return Number(y) === Number(year) && Number(m) === Number(month) && Number(d) === Number(day);
    });
}

/**** HELPERS ****/
function timeToMinutesHHMM(t) {
    const [h, m] = t.split(":").map(Number);
    return h * 60 + m;
}

function minutesToHHMM(totalMinutes) {
    const h = String(Math.floor(totalMinutes / 60)).padStart(2, "0");
    const m = String(totalMinutes % 60).padStart(2, "0");
    return `${h}:${m}`;
}

function calculateTotalSlots(duration, workingIntervals) {
    const intervals = workingIntervals || [];
    let total = 0;
    for (const i of intervals) {
        const start = timeToMinutesHHMM(i.start);
        const end = timeToMinutesHHMM(i.end);
        total += Math.floor((end - start) / duration);
    }
    return total;
}

function formatLocalYYYYMMDD(dateObj) {
    const y = dateObj.getFullYear();
    const m = String(dateObj.getMonth() + 1).padStart(2, "0");
    const d = String(dateObj.getDate()).padStart(2, "0");
    return `${y}-${m}-${d}`;
}

function getLayoutMode() {
    const w = window.innerWidth;
    if (w < 768) return "mobile";
    if (w < 1024) return "tablet";
    return "desktop";
}





/****** ADD OFFICE HOURS ********/

// Fill Options with hours
$(".daySelect").each(function () {

    var $select = $(this);

    $select.append('<option value=""></option>');

    for (var hour = 0; hour < 24; hour++) {

        var h = hour.toString().padStart(2, '0');

        $select.append('<option value="' + h + ':00">' + h + ':00</option>');
        $select.append('<option value="' + h + ':30">' + h + ':30</option>');
    }

});

// Default toggle κλειστά
$(".day-div").each(function () {
    const $d = $(this);

    $d.find(".toggle")
        .removeClass("on")
        .addClass("off");

    $d.find(".ranges").hide();

    $d.find(".daySelect")
        .prop("disabled", true)
        .val("");

    $d.find(".status-text").text("Κλειστά");
});

// Toggle ανοιχτά/κλειστά
$(function () {

    $(".day-div").each(function () {
        const $day = $(this);
        $day.find(".ranges").hide();
        $day.find(".daySelect").prop("disabled", true);
    });

    $(".toggle").on("click", function () {

        const $toggle = $(this);
        const $dayDiv = $toggle.closest(".day-div");
        const $ranges = $dayDiv.find(".ranges");
        const $statusText = $dayDiv.find(".status-text");
        const $selects = $dayDiv.find(".daySelect");

        if ($toggle.hasClass("off")) {
            $toggle.removeClass("off").addClass("on");
            $statusText.text("Ανοιχτά");

            $ranges.stop(true, true).slideDown(200);
            $selects.prop("disabled", false);

        } else {
            $toggle.removeClass("on").addClass("off");
            $statusText.text("Κλειστά");

            $ranges.stop(true, true).slideUp(200);
            $selects.prop("disabled", true).val("");
        }

    });

});

/******* EDIT PROFILE *********/

const wrapper = document.querySelector(".image-wrapper");
const input = document.getElementById("imageInput");
const preview = document.getElementById("previewImage");

if (wrapper && input) {
    wrapper.addEventListener("click", () => input.click());
}

if (input) {
    input.addEventListener("change", function () {
        const file = this.files[0];
        if (file && preview) {
            const reader = new FileReader();
            reader.onload = e => preview.src = e.target.result;
            reader.readAsDataURL(file);
        }
    });
}

const textarea = document.getElementById("bioInput");

function autoResize() {
    if (!textarea) return;

    textarea.style.height = "auto";
    textarea.style.height = textarea.scrollHeight + "px";
}

if (textarea) {
    textarea.addEventListener("input", autoResize);
    window.addEventListener("load", autoResize);
}



/******** GOOGLE PLACES ************/

        window.initOfficeAddressAutocomplete = async function () {
    console.log("Initializing custom Office Address Autocomplete");

    const wrapper = document.querySelector(".address-autocomplete");
    const input = document.getElementById("officeAddress");
    const suggestionsEl = document.getElementById("officeSuggestions");

    if (!wrapper || !input || !suggestionsEl) {
        console.log("Address autocomplete elements not found, skipping");
        return;
    }

    if (input.dataset.autocompleteInitialized === "true") {
        return;
    }

    input.dataset.autocompleteInitialized = "true";

    const { AutocompleteSuggestion, AutocompleteSessionToken } =
        await google.maps.importLibrary("places");

    let autocompleteToken = new AutocompleteSessionToken();
    let currentSuggestions = [];
    let activeIndex = -1;
    let debounceTimer = null;

    function resetSession() {
        autocompleteToken = new AutocompleteSessionToken();
    }

    function escapeHtml(str) {
        return String(str)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    function clearSuggestions() {
        currentSuggestions = [];
        activeIndex = -1;
        suggestionsEl.innerHTML = "";
        suggestionsEl.hidden = true;
    }

    function updateActiveItem() {
        const items = suggestionsEl.querySelectorAll(".suggestion-item");
        items.forEach(function (item, idx) {
            item.classList.toggle("active", idx === activeIndex);
        });
    }

    function renderSuggestions(suggestions) {
        currentSuggestions = suggestions || [];
        activeIndex = -1;
        suggestionsEl.innerHTML = "";

        if (!currentSuggestions.length) {
            clearSuggestions();
            return;
        }

        currentSuggestions.forEach(function (suggestion, index) {
            const prediction = suggestion.placePrediction;
            const text = prediction.text?.toString?.() || "";
            const secondaryText =
                prediction.structuredFormat?.secondaryText?.text || "";

            const item = document.createElement("div");
            item.className = "suggestion-item";
            item.dataset.index = index;

            item.innerHTML =
                '<div class="suggestion-main">' + escapeHtml(text) + '</div>' +
                (secondaryText
                    ? '<div class="suggestion-secondary">' + escapeHtml(secondaryText) + '</div>'
                    : '');

            item.addEventListener("mousedown", async function (e) {
                e.preventDefault();
                await selectSuggestion(index);
            });

            suggestionsEl.appendChild(item);
        });

        suggestionsEl.hidden = false;
    }

    async function fetchSuggestions(query) {
        if (!query || !query.trim()) {
            clearSuggestions();
            return;
        }

        const response =
            await AutocompleteSuggestion.fetchAutocompleteSuggestions({
                input: query,
                sessionToken: autocompleteToken
            });

        renderSuggestions(response.suggestions || []);
    }

    async function selectSuggestion(index) {
        const suggestion = currentSuggestions[index];
        if (!suggestion) return;

        const place = suggestion.placePrediction.toPlace();

        await place.fetchFields({
            fields: ["formattedAddress", "location", "addressComponents"]
        });

        const formattedAddress = place.formattedAddress || "";
        const lat = place.location?.lat?.() ?? "";
        const lng = place.location?.lng?.() ?? "";

        input.value = formattedAddress;

        const fullAddressEl = document.getElementById("FullAddress");
        const latEl = document.getElementById("Lat");
        const lonEl = document.getElementById("Lon");

        if (fullAddressEl) fullAddressEl.value = formattedAddress;
        if (latEl) latEl.value = lat;
        if (lonEl) lonEl.value = lng;

        clearSuggestions();
        resetSession();
    }

    input.addEventListener("input", function () {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(function () {
            fetchSuggestions(input.value).catch(console.error);
        }, 250);
    });

    input.addEventListener("keydown", async function (e) {
        if (suggestionsEl.hidden) return;

        if (e.key === "ArrowDown") {
            e.preventDefault();
            activeIndex = Math.min(activeIndex + 1, currentSuggestions.length - 1);
            updateActiveItem();
        } else if (e.key === "ArrowUp") {
            e.preventDefault();
            activeIndex = Math.max(activeIndex - 1, 0);
            updateActiveItem();
        } else if (e.key === "Enter") {
            if (activeIndex >= 0) {
                e.preventDefault();
                await selectSuggestion(activeIndex);
            }
        } else if (e.key === "Escape") {
            clearSuggestions();
        }
    });

    document.addEventListener("click", function (e) {
        if (!e.target.closest(".address-autocomplete")) {
            clearSuggestions();
        }
    });

    resetSession();
};