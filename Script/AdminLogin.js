<script>
    function login() {
            const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    // Example check for hardcoded credentials
    if (username === "" && password === "") {
        document.getElementById("error").textContent = "Invalid username or password";
            }
        }
</script>