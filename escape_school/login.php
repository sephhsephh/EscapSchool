<?php
require 'config.php';

header("Content-Type: application/json");

// Get POST data (works with both raw JSON and form-data)
$email = $_POST['email'] ?? null;
$password = $_POST['password'] ?? null;

if (!$email || !$password) {
    echo json_encode([
        "success" => false,
        "message" => "Email and password are required",
        "user" => null
    ]);
    exit();
}

$stmt = $conn->prepare("SELECT id, email, first_name, last_name, role, password FROM users WHERE email = ?");
$stmt->bind_param("s", $email);
$stmt->execute();
$result = $stmt->get_result();

$response = [
    "success" => false,
    "message" => "Login failed",
    "user" => null
];

if ($result->num_rows > 0) {
    $user = $result->fetch_assoc();
    if (password_verify($password, $user['password'])) {
        unset($user['password']); // Remove password before sending
        $response = [
            "success" => true,
            "message" => "Welcome, " . $user['first_name'],
            "user" => $user
        ];
    } else {
        $response["message"] = "Invalid password";
    }
} else {
    $response["message"] = "User not found";
}

echo json_encode($response);
$stmt->close();
$conn->close();
