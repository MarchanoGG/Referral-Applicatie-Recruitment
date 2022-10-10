<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>

  <header class="bg-light d-flex flex-column flex-shrink-0 bg-light" style="width: 280px;">


    <div class="h-100 d-flex flex-column flex-shrink-0 p-3 text-bg-dark" style="width: 280px;">
      <a href="/" class="d-flex align-items-center mb-3 mb-md-0 me-md-auto text-white text-decoration-none" style="margin-left: 10px;">
        <img src="https://www.alten.com/wp-content/uploads/2019/01/cropped-favicon-alten-1-180x180.png" alt="" width="34" height="32" class="rounded-circle me-2">
        <span class="fs-4">Alten</span>
      </a>
      <hr>
      <ul class="nav nav-pills flex-column mb-auto">
        <li class="nav-item">
          <RouterLink to="/" class="nav-link text-white">          
            <i class="bi bi-house-door-fill"></i>
            Home
          </RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink to="/login" class="nav-link text-white">          
            <i class="bi bi-person-circle"></i>
            Login
          </RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink to="/users" class="nav-link text-white">          
            <i class="bi bi-list"></i>
            Users
          </RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink to="/help" class="nav-link text-white">          
            <i class="bi bi-question-square-fill"></i>
            Hulp
          </RouterLink>
        </li>
      </ul>
      <hr>
      <div class="dropdown">
        <a href="#" class="d-flex align-items-center text-white text-decoration-none dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
          <img src="https://pv-c.nl/wp-content/uploads/2011/08/person-placeholder.jpg" alt="" width="32" height="32" class="rounded-circle me-2">
          <strong>Mervin de OG</strong>
        </a>
        <ul class="dropdown-menu dropdown-menu-dark text-small shadow">
          <li><a class="dropdown-item" href="#">Settings</a></li>
          <li><a class="dropdown-item" href="#">Profile</a></li>
          <li><hr class="dropdown-divider"></li>
          <li><a class="dropdown-item" href="#">Sign out</a></li>
        </ul>
      </div>
    </div>
  </header>

  <RouterView />
</template>



<style>
* {
  text-align: unset !important;;
}
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif !important;
  -webkit-font-smoothing: antialiased !important;
  -moz-osx-font-smoothing: grayscale !important;
  color: #2c3e50 !important;
  width: 100vw !important;
  height: 100vh !important;
  margin: 0 !important;
  padding: 0 !important;
  max-width: unset !important;
  display: flex !important;

}
header {
  height: 100vh;
  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
}
.nav-link {
  border-radius: 5px !important;
}

.nav-link i {
  color: white;
  font-size: 16px;
}
.nav .nav-link.router-link-exact-active {
  color: #fff !important;
  background-color: #6c757d !important;
}

</style>