document.addEventListener('DOMContentLoaded', function () {
    let projectCount = 0;

    // Initialize with one project entry
    addProjectEntry();

    // Image preview functionality
    function previewImage(input, imgElement) {
        const file = input.files[0];
        if (file) {
            const url = URL.createObjectURL(file);
            imgElement.src = url;
            imgElement.style.display = 'block';
        } else {
            imgElement.style.display = 'none';
        }
    }

    // Add new project entry
    function addProjectEntry() {
        projectCount++;
        const container = document.getElementById('projectsContainer');

        const entryDiv = document.createElement('div');
        entryDiv.className = 'project-entry mb-4';
        entryDiv.dataset.projectId = projectCount;

        entryDiv.innerHTML = `
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label">Project ${projectCount} Name*</label>
                    <input type="text" class="form-control" id="projectName${projectCount}" required>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">Project ${projectCount} Year*</label>
                    <input type="text" class="form-control" id="projectYear${projectCount}" required>
                </div>
            </div>
            <div class="mb-3">
                <label class="form-label">Project ${projectCount} Description*</label>
                <textarea class="form-control" id="projectDesc${projectCount}" rows="3" required
                    placeholder="E.g., A React-based e-commerce site with payment integration."></textarea>
            </div>
            <div class="mb-3">
                <label class="form-label">Project ${projectCount} Link</label>
                <input type="url" class="form-control" id="projectLink${projectCount}" placeholder="https://example.com">
            </div>
            <div class="mb-3">
                <label class="form-label">Project ${projectCount} Image</label>
                <input type="file" class="form-control project-image-input" id="projectImg${projectCount}" accept="image/*">
                <img id="projectPreview${projectCount}" class="img-preview" style="display:none;" alt="Project ${projectCount} Preview">
            </div>
            <button type="button" class="btn btn-sm btn-outline-danger remove-project">
                <i class="bi bi-trash"></i> Remove Project
            </button>
        `;
        container.appendChild(entryDiv);

        // Attach event listeners
        const removeBtn = entryDiv.querySelector('.remove-project');
        removeBtn.addEventListener('click', () => {
            entryDiv.remove();
        });

        const imgInput = entryDiv.querySelector('.project-image-input');
        const imgPreview = entryDiv.querySelector(`#projectPreview${projectCount}`);
        imgInput.addEventListener('change', (e) => previewImage(e.target, imgPreview));
    }

    // Form submission
    document.getElementById('portfolioForm').addEventListener('submit', async function (e) {
        e.preventDefault();

        // Validate required fields
        const requiredFields = document.querySelectorAll('[required]');
        let valid = true;

        requiredFields.forEach(field => {
            if (!field.value.trim()) {
                field.classList.add('is-invalid');
                valid = false;
            } else {
                field.classList.remove('is-invalid');
            }
        });

        if (!valid) {
            alert('Please fill all required fields');
            return;
        }

        // Gather data into FormData
        const formData = new FormData();
        formData.append('firstName', document.getElementById('firstName').value);
        formData.append('lastName', document.getElementById('lastName').value);
        formData.append('phone', document.getElementById('phone').value);
        formData.append('email', document.getElementById('email').value);
        formData.append('linkedin', document.getElementById('linkedin').value);
        formData.append('github', document.getElementById('github').value);
        formData.append('summary', document.getElementById('summary').value);

        const profileFile = document.getElementById('profileImage').files[0];
        if (profileFile) formData.append('profileImage', profileFile);

        formData.append('services', document.getElementById('services').value);
        formData.append('skills', document.getElementById('skills').value);

        // Append each project
        document.querySelectorAll('.project-entry').forEach((entry, idx) => {
            const pid = entry.dataset.projectId;
            formData.append(`projects[${idx}][name]`, document.getElementById(`projectName${pid}`).value);
            formData.append(`projects[${idx}][year]`, document.getElementById(`projectYear${pid}`).value);
            formData.append(`projects[${idx}][description]`, document.getElementById(`projectDesc${pid}`).value);
            formData.append(`projects[${idx}][link]`, document.getElementById(`projectLink${pid}`).value);

            const pf = document.getElementById(`projectImg${pid}`).files[0];
            if (pf) formData.append(`projects[${idx}][image]`, pf);
        });

        console.log('Form data to be sent:', formData);

        // Here you would typically send to your backend
        // Example:
        // try {
        //     const response = await fetch('/api/portfolio', {
        //         method: 'POST',
        //         body: formData
        //     });
        //     const result = await response.json();
        //     console.log('Success:', result);
        //     alert('Portfolio created successfully!');
        // } catch (error) {
        //     console.error('Error:', error);
        //     alert('Error creating portfolio');
        // }

        alert('Portfolio data ready to be sent! Check console for data.');
    });

    // Event listeners
    document.getElementById('addProjectBtn').addEventListener('click', addProjectEntry);

    document.getElementById('profileImage').addEventListener('change', (e) => {
        const imgEl = document.getElementById('profilePreview');
        previewImage(e.target, imgEl);
    });
});