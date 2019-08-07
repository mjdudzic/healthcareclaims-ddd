DROP TABLE IF EXISTS batches;
DROP TABLE IF EXISTS batch_submission_statuses;

CREATE TABLE batch_submission_statuses (
	id INT PRIMARY KEY,
	name VARCHAR (200) NOT NULL
);

CREATE TABLE batches (
	id UUID PRIMARY KEY,
	batch_submission_status_id INT NOT NULL,
	batch_uri VARCHAR (2000) NOT NULL,
	feedback_uri VARCHAR (2000) NULL,
	creation_date TIMESTAMP NOT NULL,

	FOREIGN KEY (batch_submission_status_id) REFERENCES batch_submission_statuses (id)
);


