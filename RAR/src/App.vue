<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>




  <header class="bg-light d-flex flex-column flex-shrink-0 bg-light">

    <div class="h-100 d-flex flex-column flex-shrink-0 bg-light" style="width: 4.5rem;">

    <ul class="nav nav-pills nav-flush flex-column mb-auto text-center">

      <li>
        <RouterLink to="/" class="nav-link">          
          <i class="bi bi-house-door-fill"></i>
        </RouterLink>
      </li>

      <li>
        <RouterLink to="/login" class="nav-link">          
          <i class="bi bi-person-circle"></i>
        </RouterLink>
      </li>

    </ul>


    <div class="nav-info dropdown border-top">
      <a href="#" class="d-flex align-items-center justify-content-center p-3 link-dark text-decoration-none dropdown-toggle" id="dropdownUser3" data-bs-toggle="dropdown" aria-expanded="false">
        <img src="https://pv-c.nl/wp-content/uploads/2011/08/person-placeholder.jpg" alt="mdo" width="24" height="24" class="rounded-circle">
      </a>
      <ul class="dropdown-menu text-small shadow" aria-labelledby="dropdownUser3" style="">
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

#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  width: 100vw;
  height: 100vh;
  margin: 0; 
  padding: 0;
  max-width: unset;
  display: block;
}
header {
  position: fixed;
  height: 100vh;
  left: 0;
  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
}
.nav-link {
  border-radius: 0 !important;
}

.nav-link i {
  color: black;
  font-size: 24px;
}
.nav .nav-link.router-link-exact-active {
  color: #fff !important;
  background-color: #007bff;
  border-color: #007bff;
}

</style>