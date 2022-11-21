<script setup>
import axios from 'axios';
</script>

<template>

    <div id="login">
        <h1>Login</h1>

        <ul v-if="users && users.length">
            <li v-for="user of users">
                <p>{{user.username}}</p>
                <p>{{user.password}}</p>
            </li>
        </ul>

        <p class="text-danger">{{ warning }}</p>
        <div class="form-inputs">
            <label for="username">Username</label><br>
            <input type="text"  class="form-control" id="username" name="username" v-model="input.username" placeholder="Username" />
        </div><br>
        <div class="form-inputs">
            <label for="password">Password</label><br>
            <input type="password"  class="form-control" id="password" name="password" v-model="input.password" placeholder="Password" />
        </div><br>
        <button type="button" class="btn btn-primary" v-on:click="login()">Login</button>
    </div>
        
</template>

<script>
    export default {
        name: 'Login',
        data() {
            return {
                input: {
                    username: "",
                    password: ""
                },
                warning: "",
                users: [],
                errors: []
            }
        },
        methods: {
            login() {
                // if(this.input.username != "" && this.input.password != "") {

                    // Get users from API and store in users[]
                    axios
                    .get('http://127.0.0.1:5173/tests/api/users')
                    .then(response => (this.users = response.data))
                    .catch(error => (this.errors = error))


                    // Check if username and password match
                    for (let i = 0; i < this.users.length; i++) {
                        if(this.users[i].username == this.input.username && this.users[i].password == this.input.password) {
                            this.warning = "Login successful";
                            return;
                        }
                    }

                    // for (let i = 0; i < this.users.length; i++) {
                    //     console.log(this.users[i].username);
                    //     if(this.users[i].email == this.input.username && this.users[i].password == this.input.password) {
                    //         // this.$router.push({ name: 'Home' })
                    //         this.warning = "CORRECT!!"
                    //         console.log("CORRECT!!")
                    //     } else {
                    //         this.warning = "Username or password is incorrect"
                    //     }
                    // }

                // } else {
                //     this.warning = "Vul alle velden in";
                // }
            }
        }
    } 
</script>

<style>
h1 {
    font-size: 30px;
}
#login {
    width: 400px;
    position: absolute;
    top: 50%;
    left: calc(50% + 80px);
    transform: translate(-50%, -50%);
}

</style>