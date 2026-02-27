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


    // -------- Specialization dropdown with search inside (doctor) --------
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
            const $items = $list.find(".dropdown-item"); // jQuery collection
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

/******WEEK SCROLLER*******/
    function showOffice(officeId) {
        document.querySelectorAll('.office-panel').forEach(p => p.style.display = 'none');
    const panel = document.getElementById('office-' + officeId);
    if (panel) panel.style.display = 'block';

    document.querySelectorAll('.office-tab').forEach(b => {
        b.classList.toggle('active', b.dataset.officeId == officeId);
    });
  }

    function scrollDays(direction, officeId) {
    const scroller = document.getElementById('daysScroller-' + officeId);
    if (!scroller) return;
    scroller.scrollBy({left: direction * 320, behavior: 'smooth' });
  }

    function pickSlot(officeId, dateIso, slot) {
        console.log({ officeId, dateIso, slot });
    // set hidden inputs or submit form here
  }

function scrollDays(dir) {
    const el = document.getElementById('daysScroller');
    if (!el) return;
    el.scrollLeft += dir * 220; // ~1 column per click
}

function selectDay(isoDate) {
    // Frontend-only mode: just log.
    // When backend is ready, you can reload with ?date=isoDate or fetch slots.
    console.log('Selected day:', isoDate);

    /* Optional: visually mark selected day client-side (no reload)
    const cols = document.querySelectorAll('#daysScroller .day-col');
    cols.forEach(c => c.classList.remove('selected'));
    const target = document.querySelector(`#daysScroller .day-col[data-date="${isoDate}"]`);
    if (target) target.classList.add('selected');
    */
}

function pickSlot(isoDate, time) {
    console.log('Selected slot:', isoDate, time);
    // TODO later: open modal / POST reservation / navigate to booking form
}


/********************** CALENDAR ************************/

const officeCalendarState = {};

document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".calendarContainer").forEach(calendarContainer => {
        const officeId = calendarContainer.dataset.officeId;

        officeCalendarState[officeId] = {
            currentDate: new Date(),
            view: "month"
        };

        renderCurrentView(calendarContainer);
    });
});

function renderCurrentView(container) {
    const officeId = container.dataset.officeId;
    const state = officeCalendarState[officeId];

    if (state.view === "month") generateMonthView(container, state.currentDate);
    else generateDayView(container, state.currentDate);
}

/** ---------------------------
 *  CONFIG (later you’ll fetch from backend per office/doctor)
 *  --------------------------- */
function getOfficeScheduleConfig(officeId, dateStr) {
    // TODO: replace with backend response
    return {
        duration: 30,
        workingIntervals: [
            { start: "09:00", end: "14:00" },
            { start: "17:00", end: "21:00" }
        ]
    };
}

/**** DAY VIEW ****/
function generateDayView(calendarContainer, dateObj) {
    const officeId = calendarContainer.dataset.officeId;

    const dayNames = ["Κυριακή", "Δευτέρα", "Τρίτη", "Τετάρτη", "Πέμπτη", "Παρασκευή", "Σάββατο"];
    const monthNames = [
        "Ιανουαρίου", "Φεβρουαρίου", "Μαρτίου", "Απριλίου", "Μαϊου", "Ιουνίου",
        "Ιουλίου", "Αυγούστου", "Σεπτεμβρίου", "Οκτωβρίου", "Νοεμβρίου", "Δεκεμβρίου"
    ];

    // heading (prev-next + current date)
    const oldHeading = calendarContainer.querySelector(".month");
    if (oldHeading) oldHeading.remove();

    const headingElement = document.createElement("div");
    headingElement.classList.add("month");
    headingElement.innerText =
        `${dayNames[dateObj.getDay()]} ${dateObj.getDate()} ${monthNames[dateObj.getMonth()]} ${dateObj.getFullYear()}`;

    // nav
    const navButtons = calendarContainer.querySelector(".nav-buttons-container");
    navButtons.innerHTML = "";

    const prevButton = document.createElement("button");
    prevButton.innerHTML = "‹";

    const nextButton = document.createElement("button");
    nextButton.innerHTML = "›";

    prevButton.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        st.currentDate = new Date(st.currentDate);
        st.currentDate.setDate(st.currentDate.getDate() - 1);
        renderCurrentView(calendarContainer);
    });

    nextButton.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        st.currentDate = new Date(st.currentDate);
        st.currentDate.setDate(st.currentDate.getDate() + 1);
        renderCurrentView(calendarContainer);
    });

    navButtons.appendChild(prevButton);
    navButtons.appendChild(headingElement);
    navButtons.appendChild(nextButton);

    // inner container
    const inner = calendarContainer.querySelector(".calendarInnerContainer");
    inner.innerHTML = "";

    // back to month button
    const backMonthBtn = document.createElement("button");
    backMonthBtn.className = "fs-14 bg-transparent fc-blue mb-3 border-0";
    backMonthBtn.textContent = "‹ Προβολή όλου του μήνα";
    backMonthBtn.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        st.view = "month";
        renderCurrentView(calendarContainer);
    });

    // day container
    const dayDiv = document.createElement("div");
    dayDiv.classList.add("dayView");

    const dateStr = formatLocalYYYYMMDD(dateObj);
    dayDiv.dataset.date = dateStr;
    dayDiv.dataset.officeId = officeId;

    inner.appendChild(backMonthBtn);
    inner.appendChild(dayDiv);

    fetchAppointmentsForDate(officeId, dateStr, dayDiv);
}

/** ---------------------------
 *  DAY RENDERER (ONLY working intervals + separator)
 *  --------------------------- */
function displayAppointmentsForDate(appointments, dayDiv, duration, workingIntervals) {
    if (!Array.isArray(appointments) || !dayDiv) return;

    dayDiv.innerHTML = "";

    const apptByTime = new Map();
    appointments.forEach(a => {
        if (a && a.time) apptByTime.set(a.time.split('T')[1].slice(0, 5), a);
    });

    const slotsContainer = document.createElement("div");
    slotsContainer.classList.add("slots-container");

    const intervals = [...(workingIntervals || [])].sort(
        (a, b) => timeToMinutesHHMM(a.start) - timeToMinutesHHMM(b.start)
    );

    for (let i = 0; i < intervals.length; i++) {
        const interval = intervals[i];
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
                name.textContent = appt.patientFirstName + " " + appt.patientLastName || "Appointment";
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

        // separator between intervals
        if (i < intervals.length - 1) {
            const next = intervals[i + 1];
            const separator = document.createElement("div");
            separator.classList.add("interval-separator");
            separator.textContent = `Διάλειμμα ${interval.end} - ${next.start}`;
            slotsContainer.appendChild(separator);
        }
    }

    dayDiv.appendChild(slotsContainer);
}

/** ---------------------------
 *  MONTH VIEW
 *  --------------------------- */
async function generateMonthView(calendarContainer, dateObj) {
    const officeId = calendarContainer.dataset.officeId;

    const inner = calendarContainer.querySelector(".calendarInnerContainer");
    const navButtons = calendarContainer.querySelector(".nav-buttons-container");
    if (!inner || !navButtons) return;

    const oldHeading = calendarContainer.querySelector(".month");
    if (oldHeading) oldHeading.remove();

    inner.innerHTML = "";

    const monthNames = [
        "Ιανουάριος", "Φεβρουάριος", "Μάρτιος", "Απρίλιος", "Μάιος", "Ιούνιος",
        "Ιούλιος", "Αύγουστος", "Σεπτέμβριος", "Οκτώβριος", "Νοέμβριος", "Δεκέμβριος"
    ];
    const weekDaysShort = ["Δευ", "Τρι", "Τετ", "Πεμ", "Παρ", "Σαβ", "Κυρ"];

    const year = dateObj.getFullYear();
    const month = dateObj.getMonth();

    const headingElement = document.createElement("div");
    headingElement.classList.add("month");
    headingElement.innerText = `${monthNames[month]} ${year}`;

    // nav
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
        renderCurrentView(calendarContainer);
    });

    nextButton.addEventListener("click", () => {
        const st = officeCalendarState[officeId];
        const d = new Date(st.currentDate);
        d.setMonth(d.getMonth() + 1);
        st.currentDate = d;
        renderCurrentView(calendarContainer);
    });

    // layout mode
    const mode = getLayoutMode();

    const grid = document.createElement("div");
    grid.classList.add(mode === "mobile" ? "month-list" : "month-grid");
    if (mode === "tablet") grid.classList.add("month-grid-tablet");

    // weekday headers
    if (mode !== "mobile") {
        weekDaysShort.forEach(w => {
            const h = document.createElement("div");
            h.classList.add("month-header");
            h.textContent = w;
            grid.appendChild(h);
        });
    }

    // Monday-first fix
    let firstDay = new Date(year, month, 1).getDay(); // 0=Sun
    firstDay = (firstDay + 6) % 7; // Monday=0

    const daysInMonth = new Date(year, month + 1, 0).getDate();

    // empty cells
    if (mode !== "mobile") {
        for (let i = 0; i < firstDay; i++) {
            const empty = document.createElement("div");
            empty.classList.add("month-cell", "empty");
            grid.appendChild(empty);
        }
    }

    let monthFirst = `${year}-${String(month + 1).padStart(2, "0")}-01`;
    var appointments;
    await fetchAppointmentsForMonth(officeId, monthFirst).then(x => appointments = x);

    for (let d = 1; d <= daysInMonth; d++) {
        const cell = document.createElement("div");
        cell.classList.add("month-cell");

        const dateStr = `${year}-${String(month + 1).padStart(2, "0")}-${String(d).padStart(2, "0")}`;
        const closed = isOfficeClosed(officeId, dateStr);

        if (closed) cell.classList.add("month-cell-closed");

        const dayNum = document.createElement("div");
        dayNum.classList.add("month-day-number");
        dayNum.textContent = d;
        cell.appendChild(dayNum);

        // Summary (only if open)
        if (!closed) {
            const { duration, workingIntervals } = getOfficeScheduleConfig(officeId, dateStr);

            let appts = getAppointmentsByDay(appointments, year, month+1, d);
            //console.log(appts);

            //await fetchAppointmentsForDay(officeId, dateStr).then(x => appts = x);
            //const appts = generateFakeAppointments(officeId, dateStr, { duration, workingIntervals });

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

        // Today highlight
        const today = new Date();
        if (year === today.getFullYear() && month === today.getMonth() && d === today.getDate()) {
            cell.classList.add("today");
        }

        if (!closed) {
            cell.style.cursor = "pointer";
            cell.addEventListener("click", () => {
                const st = officeCalendarState[officeId];
                st.currentDate = new Date(`${dateStr}T00:00:00`);
                st.view = "day";
                renderCurrentView(calendarContainer);
            });
        } else {
            cell.style.cursor = "not-allowed";
        }

        grid.appendChild(cell);
    }

    inner.appendChild(grid);
}

function fetchAppointmentsForDate(officeId, dateStr, dayDiv) {
    const { duration, workingIntervals } = getOfficeScheduleConfig(officeId, dateStr);

    //const appointments = generateFakeAppointments(officeId, dateStr, {
    //    duration,
    //    workingIntervals
    //});

    fetch('/Appointment/GetAppointmentsByDayAndPracticeId?date=' + dateStr + '&practiceId=' + officeId)
        .then(response => response.json())
        .then(appointments => {
            displayAppointmentsForDate(appointments, dayDiv, duration, workingIntervals);
        });

    //displayAppointmentsForDate(appointments, dayDiv, duration, workingIntervals);
}

async function fetchAppointmentsForDay(officeId, dateStr) {
    return await fetch('/Appointment/GetAppointmentsByDayAndPracticeId?date=' + dateStr + '&practiceId=' + officeId)
        .then(response => response.json())
        .then(appointments => {
            return appointments;
        });
}

async function fetchAppointmentsForMonth(officeId, dateStr) {
    return await fetch('/Appointment/GetAppointmentsByMonthAndPracticeId?date=' + dateStr + '&practiceId=' + officeId)
        .then(response => response.json())
        .then(appointments => {
            return appointments;
        });
}

function getAppointmentsByDay(appointments, year, month, day) {
    return appointments.filter(app => {
        const [y, m, d] = app.time.slice(0, 10).split("-");

        return (
            Number(y) === Number(year) &&
            Number(m) === Number(month) &&
            Number(d) === Number(day)
        );
    }
    );
}

function generateFakeAppointments(officeId, dateStr, options = {}) {
    const names = [
        "Maria Papadopoulos",
        "Giannis Nikolaou",
        "Eleni Georgiou",
        "Nikos Kostopoulos",
        "Katerina Ioannou",
        "Dimitris Vlachos"
    ];

    const duration = options.duration ?? 30;
    const workingIntervals = options.workingIntervals ?? [
        { start: "09:00", end: "14:00" },
        { start: "17:00", end: "21:00" }
    ];

    // Build all valid slot start times (ONLY inside intervals)
    const allSlotMins = [];
    const intervals = [...workingIntervals].sort(
        (a, b) => timeToMinutesHHMM(a.start) - timeToMinutesHHMM(b.start)
    );

    for (const intv of intervals) {
        const startMin = timeToMinutesHHMM(intv.start);
        const endMin = timeToMinutesHHMM(intv.end);
        for (let m = startMin; m < endMin; m += duration) {
            allSlotMins.push(m);
        }
    }

    if (allSlotMins.length === 0) return [];

    // Deterministic count 0..min(20, slots)
    const baseSeed = `${officeId}|${dateStr}|${duration}|${intervals.map(i => `${i.start}-${i.end}`).join(",")}`;
    const maxAppointments = Math.min(20, allSlotMins.length);
    const count = Math.floor(seededRandom(baseSeed + "|count") * (maxAppointments + 1));

    const used = new Set();
    const appointments = [];

    for (let i = 0; i < count; i++) {
        let tries = 0;
        let idx;

        do {
            idx = Math.floor(seededRandom(baseSeed + `|pick|${i}|${tries}`) * allSlotMins.length);
            tries++;
        } while (used.has(idx) && tries < 60);

        if (used.has(idx)) break;
        used.add(idx);

        const mins = allSlotMins[idx];
        const timeStr = minutesToHHMM(mins);

        const userName = names[Math.floor(seededRandom(baseSeed + `|name|${i}`) * names.length)];

        appointments.push({
            id: Number(`${officeId}${dateStr.replaceAll("-", "")}${i}`.slice(0, 12)),
            user_name: userName,
            time: timeStr
        });
    }

    appointments.sort((a, b) => a.time.localeCompare(b.time));
    return appointments;
}

function seededRandom(seedStr) {
    let h = 2166136261;
    for (let i = 0; i < seedStr.length; i++) {
        h ^= seedStr.charCodeAt(i);
        h = Math.imul(h, 16777619);
    }
    h += h << 13; h ^= h >>> 7; h += h << 3; h ^= h >>> 17; h += h << 5;
    return (h >>> 0) / 4294967296;
}

function getOfficeScheduleConfig(officeId, dateStr) {
    const d = new Date(`${dateStr}T00:00:00`);
    const dow = d.getDay(); // 0=Sun ... 6=Sat

    // default duration (fixed per doctor/office)
    const duration = 30;

    // different schedules for testing
    if (dow === 1) { // Monday
        return {
            duration,
            workingIntervals: [
                { start: "09:00", end: "14:00" },
                { start: "17:00", end: "21:00" }
            ]
        };
    }

    if (dow === 2) { // Tuesday
        return {
            duration: 20,
            workingIntervals: [
                { start: "08:00", end: "12:00" },
                { start: "16:00", end: "19:00" }
            ]
        };
    }

    if (dow === 3) { // Wednesday (single continuous)
        return {
            duration,
            workingIntervals: [
                { start: "10:00", end: "18:00" }
            ]
        };
    }

    if (dow === 4) { // Thursday (3 intervals)
        return {
            duration: 15,
            workingIntervals: [
                { start: "09:00", end: "11:00" },
                { start: "12:00", end: "14:00" },
                { start: "18:00", end: "20:00" }
            ]
        };
    }

    if (dow === 5) { // Friday (late shift)
        return {
            duration,
            workingIntervals: [
                { start: "12:00", end: "20:00" }
            ]
        };
    }

    if (dow === 6) { // Saturday (short)
        return {
            duration,
            workingIntervals: [
                { start: "09:00", end: "13:00" }
            ]
        };
    }

    // Sunday closed (no intervals)
    return { duration, workingIntervals: [] };
}

/*** REAL DATA ***
async function fetchAppointmentsForDate(officeId, dateStr, dayDiv) {
    const res = await fetch(`/api/offices/${officeId}/schedule/day?date=${dateStr}`, {
        headers: { "Accept": "application/json" }
    });

    if (!res.ok) {
        console.error("Failed to load schedule", await res.text());
        dayDiv.innerHTML = "<p>Failed to load schedule</p>";
        return;
    }

    const data = await res.json();
    const { duration, workingIntervals, appointments } = data;

    displayAppointmentsForDate(appointments, dayDiv, duration, workingIntervals);
}
*/
/****  HELPERS ****/
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

function isOfficeClosed(officeId, dateStr) {
    const d = new Date(`${dateStr}T00:00:00`);
    const isSunday = d.getDay() === 0;

    const r = seededRandom(`${officeId}|${dateStr}|closed`);
    const randomClosed = r < 0.10;

    return isSunday || randomClosed;
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