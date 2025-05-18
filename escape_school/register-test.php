<?php require 'config-test.php'; ?>
<!DOCTYPE html>
<html>

<head>
    <title>Register</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            max-width: 400px;
            margin: 0 auto;
            padding: 20px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            display: block;
            margin-bottom: 5px;
        }

        input {
            width: 100%;
            padding: 8px;
            box-sizing: border-box;
        }

        button {
            background: #4CAF50;
            color: white;
            padding: 10px;
            border: none;
            width: 100%;
        }

        .error {
            color: red;
        }

        .success {
            color: green;
        }
    </style>
</head>

<body>
    <h2>Register</h2>

    <?php
    if ($_SERVER["REQUEST_METHOD"] == "POST") {
        $email = $_POST['email'];
        $password = password_hash($_POST['password'], PASSWORD_BCRYPT);
        $firstName = $_POST['first_name'];
        $lastName = $_POST['last_name'];
        $role = 'user';
        $verified = 0;

        // Check if email exists
        $check = $conn->prepare("SELECT id FROM users WHERE email = ?");
        $check->bind_param("s", $email);
        $check->execute();
        $check->store_result();

        if ($check->num_rows > 0) {
            echo '<p class="error">Email already registered!</p>';
        } else {
            $stmt = $conn->prepare("INSERT INTO users (email, password, first_name, last_name, role, verified) VALUES (?, ?, ?, ?, ?, ?)");
            $stmt->bind_param("sssssi", $email, $password, $firstName, $lastName, $role, $verified);

            if ($stmt->execute()) {
                echo '<p class="success">Registration successful! <a href="login.php">Login now</a></p>';
            } else {
                echo '<p class="error">Registration failed: ' . $conn->error . '</p>';
            }
        }
    }
    ?>

    <form method="post">
        <div class="form-group">
            <label>Email:</label>
            <input type="email" name="email" required>
        </div>
        <div class="form-group">
            <label>Password:</label>
            <input type="password" name="password" required>
        </div>
        <div class="form-group">
            <label>First Name:</label>
            <input type="text" name="first_name" required>
        </div>
        <div class="form-group">
            <label>Last Name:</label>
            <input type="text" name="last_name" required>
        </div>
        <button type="submit">Register</button>
    </form>

    <p>Already have an account? <a href="login.php">Login here</a></p>
</body>

</html>