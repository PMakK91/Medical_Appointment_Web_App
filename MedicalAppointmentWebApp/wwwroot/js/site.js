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