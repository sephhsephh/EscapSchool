<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");

include 'config.php';

$response = array();
$response['success'] = false;
$response['message'] = '';
$response['questions'] = array();

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $difficulty = isset($_POST['difficulty']) ? $_POST['difficulty'] : 'easy';
    
    try {
        $stmt = $conn->prepare("SELECT * FROM questions WHERE difficulty = ?");
        $stmt->bind_param("s", $difficulty);
        $stmt->execute();
        $result = $stmt->get_result();
        
        if ($result->num_rows > 0) {
            while ($row = $result->fetch_assoc()) {
                $response['questions'][] = array(
                    'id' => $row['id'],
                    'question' => $row['question'],
                    'answer' => $row['answer'],
                    'difficulty' => $row['difficulty']
                );
            }
            $response['success'] = true;
            $response['message'] = 'Questions retrieved successfully';
        } else {
            $response['message'] = 'No questions found for the specified difficulty';
        }
    } catch (Exception $e) {
        $response['message'] = 'Error: ' . $e->getMessage();
    }
} else {
    $response['message'] = 'Invalid request method';
}

echo json_encode($response);
?>