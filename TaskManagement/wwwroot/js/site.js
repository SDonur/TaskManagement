document.addEventListener('DOMContentLoaded', () => {
    getTasks();
  
    document.getElementById('taskForm').addEventListener('submit', async function (e) {
        e.preventDefault();
        const title = document.getElementById('title').value;

        await addTask(title);

        getTasks();
    });
});

//görev listesi
async function getTasks() {
    
    const token = localStorage.getItem('token');
    const response = await fetch('https://localhost:7079/api/TaskManagement', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    
    if (response.ok) {
        const tasks = await response.json();
        const taskList = document.getElementById('taskList');
        taskList.innerHTML = '';

        tasks.forEach(task => {
            const li = document.createElement('li');
            li.textContent = `${task.title} - ${task.isCompleted ? 'Tamamlandı' : 'Tamamlanmadı'}`;
            li.className = 'list-group-item';
            li.id = `task-${task.id}`;

            const updateButton = document.createElement('button');
            updateButton.textContent = 'Güncelle';
            updateButton.className = 'btn btn-warning btn-sm float-right ml-2';
            updateButton.onclick = () => updateTask(task.id);

            const deleteButton = document.createElement('button');
            deleteButton.textContent = 'Sil';
            deleteButton.className = 'btn btn-danger btn-sm float-right';
            deleteButton.onclick = () => deleteTask(task.id);

            li.appendChild(updateButton);
            li.appendChild(deleteButton);

            taskList.appendChild(li);
        });
    } else {
        alert('Görev listeleme hatası')
    }
}

//görev ekleme
async function addTask(title) {
    
    const token = localStorage.getItem('token');
    const response = await fetch('https://localhost:7079/api/TaskManagement', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ title: title, isCompleted: false })
    });

    if (!response.ok) {
        alert('Görev ekleme hatası')
    }
}

//görev güncelleme
async function updateTask(id) {
    
    const token = localStorage.getItem('token');
    const newTitle = prompt('Yeni görev başlığını girin:');
    const newStatus = confirm('Görev tamamlandı mı?') ? true : false;

    const response = await fetch(`https://localhost:7079/api/TaskManagement/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ title: newTitle, isCompleted: newStatus })
    });
    
    if (response.ok) {
        getTasks();
    } else {
        alert('Görev güncelleme hatası')
    }
}

//görev silme
async function deleteTask(id) {
    
    const token = localStorage.getItem('token');
    const response = await fetch(`https://localhost:7079/api/TaskManagement/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (response.ok) {
        getTasks();
    } else {
        alert('Görev silme hatası')
    }
}