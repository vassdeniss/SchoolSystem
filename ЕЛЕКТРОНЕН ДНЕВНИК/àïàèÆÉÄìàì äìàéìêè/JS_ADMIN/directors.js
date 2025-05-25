document.addEventListener('DOMContentLoaded', () => {
  // sidebar toggle
  const menuBtn  = document.getElementById('menu-btn');
  const sideMenu = document.getElementById('sideMenu');
  menuBtn.addEventListener('click', () => sideMenu.classList.toggle('open'));

  // elements
  const addBtn      = document.getElementById('add-btn');
  const tbody       = document.querySelector('#directorsTable tbody');
  const editPageBtn = document.getElementById('edit-page-btn');

  // demo data
  let directors = [
    { name:'Георги Димитров', from:'2024-01-01', to:'2025-12-31', status:'active', user:'gdimitrov', pass:'••••' },
    { name:'Петя Иванова',    from:'2022-09-01', to:'2024-08-31', status:'inactive', user:'pivanova',  pass:'••••' }
  ];
  let pageEditing = false;

  // render table
  function render() {
    tbody.innerHTML = '';
    directors.forEach((d,i) => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td contenteditable>${d.name}</td>
        <td contenteditable>${d.from}</td>
        <td contenteditable>${d.to}</td>
        <td>
          <select ${!pageEditing?'disabled':''} data-i="${i}">
            <option value="active"   ${d.status==='active'  ?'selected':''}>активен</option>
            <option value="inactive" ${d.status==='inactive'?'selected':''}>неактивен</option>
          </select>
        </td>
        <td contenteditable>${d.user}</td>
        <td contenteditable>${d.pass}</td>
        <td class="actions">
          <button class="save"   data-i="${i}" title="Запази">💾</button>
          <button class="delete" data-i="${i}" title="Изтрий">🗑️</button>
        </td>`;
      if (!pageEditing) {
        tr.querySelectorAll('[contenteditable]').forEach(td => td.removeAttribute('contenteditable'));
      }
      tbody.appendChild(tr);
    });

    // bind save/delete
    tbody.querySelectorAll('.save').forEach(btn => {
      btn.onclick = () => {
        const i = btn.dataset.i;
        const tr = btn.closest('tr');
        const cells = tr.querySelectorAll('td[contenteditable]');
        directors[i].name   = cells[0].textContent.trim();
        directors[i].from   = cells[1].textContent.trim();
        directors[i].to     = cells[2].textContent.trim();
        directors[i].user   = cells[3].textContent.trim();
        directors[i].pass   = cells[4].textContent.trim();
        directors[i].status = tr.querySelector('select').value;
        render();
      };
    });
    tbody.querySelectorAll('.delete').forEach(btn => {
      btn.onclick = () => {
        directors.splice(btn.dataset.i,1);
        render();
      };
    });
  }

  // add new
  addBtn.onclick = () => {
    const n = document.getElementById('inp-name').value.trim();
    const f = document.getElementById('inp-from').value;
    const t = document.getElementById('inp-to').value;
    const s = document.getElementById('inp-status').value;
    const u = document.getElementById('inp-user').value.trim();
    const p = document.getElementById('inp-pass').value.trim();
    if (n && f && t && u && p) {
      directors.push({name:n,from:f,to:t,status:s,user:u,pass:'••••'});
      document.querySelectorAll('#inp-name,#inp-from,#inp-to,#inp-user,#inp-pass').forEach(i=>i.value='');
      render();
    } else {
      alert('Попълнете всички полета');
    }
  };

  // toggle page edit mode
  editPageBtn.onclick = () => {
    pageEditing = !pageEditing;
    document.body.classList.toggle('page-editing', pageEditing);
    editPageBtn.textContent = pageEditing ? 'Запази страница' : 'Редактирай';
    // enable selects
    render();
  };

  // initial draw
  render();
});
