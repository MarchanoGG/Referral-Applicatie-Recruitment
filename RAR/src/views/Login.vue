<template>


    <div id="login">
        <h1>Inloggen</h1>
        <p class="text-danger">{{ warning }}</p>
        <div class="form-inputs">
            <label for="username">Gebruikersnaam</label><br>
            <input type="text"  class="form-control" id="username" name="username" v-model="input.username" placeholder="Username" />
        </div><br>
        <div class="form-inputs">
            <label for="password">Wachtwoord</label><br>
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
                warning: ""
            }
        },
        methods: {
            login() {
                if(this.input.username != "" && this.input.password != "") {
                    // Hier moet het inloggen gebeuren met de API
                    if(this.input.username == "admin" && this.input.password == "admin") {
                        this.$emit("authenticated", true);
                        this.$router.replace({ name: "Secure" });
                    } else {
                        this.warning = "Gebruikersnaam of wachtwoord is onjuist";
                    }
                } else {
                    this.warning = "Vul alle velden in";
                }
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